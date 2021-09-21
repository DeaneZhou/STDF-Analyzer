﻿using DataContainer;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using SillyMonkey.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace UI_Data.ViewModels {
    public class DataRawViewModel : BindableBase, INavigationAware {
        IRegionManager _regionManager;
        IEventAggregator _ea;

        SubData _subData;

        private ObservableCollection<Item> _testItems;
        public ObservableCollection<Item> TestItems {
            get { return _testItems; }
            set { SetProperty(ref _testItems, value); }
        }

        private string _header;
        public string Header {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        private string _regionName;
        public string RegionName {
            get { return _regionName; }
            set { SetProperty(ref _regionName, value); }
        }

        public DataRawViewModel(IRegionManager regionManager, IEventAggregator ea) {
            _regionManager = regionManager;
            _ea = ea;
            _ea.GetEvent<Event_FilterUpdated>().Subscribe(UpdateView);

            InitUi();
        }

        private void UpdateView(SubData data) {
            if(data.Equals(_subData)) {
                var dataAcquire = StdDB.GetDataAcquire(data.StdFilePath);
                TestItems = new ObservableCollection<Item>(dataAcquire.GetFilteredItems(data.FilterId));
                RaisePropertyChanged("TestItems");
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            var data = (SubData)navigationContext.Parameters["subData"];

            return data.Equals(_subData);
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {
            ;
        }

        public void OnNavigatedTo(NavigationContext navigationContext) {
            if (TestItems is null) {
                var data = (SubData)navigationContext.Parameters["subData"];
                _subData = data;

                var dataAcquire = StdDB.GetDataAcquire(data.StdFilePath);

                TestItems = new  ObservableCollection<Item>(dataAcquire.GetFilteredItems(data.FilterId));

                Header = $"F:{_subData.FilterId:x8}";

                RegionName = $"Region_{_subData.FilterId:x8}";
            }
        }

        private List<string> _selectedItemList= new List<string>();

        private void ShowTrend() {

            var parameters = new NavigationParameters();
            parameters.Add("itemList", _selectedItemList);
            parameters.Add("subData", _subData);


            _regionManager.RequestNavigate(RegionName, "Trend", parameters);

        }
        private void ShowHistogram() {

        }
        private void ShowBox() {

        }
        private void ShowWaferMap() {

        }
        private void ExportToExcel() {

        }

        public DelegateCommand<object> CloseCommand { get; private set; }
        public DelegateCommand ShowTrendCommand { get; private set; }
        public DelegateCommand ShowHistogramCommand { get; private set; }
        public DelegateCommand ShowBoxCommand { get; private set; }
        public DelegateCommand ShowWaferMapCommand { get; private set; }
        public DelegateCommand ExportToExcelCommand { get; private set; }


        public DelegateCommand<object> OnSelectColumn { get; private set; }
        public DelegateCommand<object> OnSelectRow { get; private set; }
        public DelegateCommand<object> OnSelectionChanged { get; private set; }

        public void InitUi() {
            CloseCommand = new DelegateCommand<object>((x)=> {
                if (_regionManager.Regions["Region_DataView"].Views.Contains(x)) {
                    _regionManager.Regions["Region_DataView"].Remove(x);
                }

            });

            ShowTrendCommand = new DelegateCommand(ShowTrend);
            ShowHistogramCommand = new DelegateCommand(ShowHistogram);
            ShowBoxCommand = new DelegateCommand(ShowBox);
            ShowWaferMapCommand = new DelegateCommand(ShowWaferMap);
            ExportToExcelCommand = new DelegateCommand(ExportToExcel);

            OnSelectionChanged = new DelegateCommand<object>((x) => {
                _selectedItemList.Clear();
                var grid = x as System.Windows.Controls.DataGrid;
                foreach(var v in grid.SelectedItems) {
                    _selectedItemList.Add((v as Item).TestNumber);
                }

                if (_selectedItemList.Count > 0) {
                    _ea.GetEvent<Event_ItemsSelected>().Publish(new Tuple<SubData, List<string>>(_subData, _selectedItemList));
                }
            });

        }



    }
}
