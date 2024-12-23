﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using SillyMonkey.Core;
using DataContainer;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace UI_DataList.ViewModels {
    public class DataFilterViewModel : BindableBase {
        IEventAggregator _ea;

        private ObservableCollection<byte> _allSites;
        public ObservableCollection<byte> AllSites {
            get { return _allSites; }
            set { SetProperty(ref _allSites, value); }
        }

        private ObservableCollection<byte> _enabledSites;
        public ObservableCollection<byte> EnabledSites {
            get { return _enabledSites; }
            set { SetProperty(ref _enabledSites, value); }
        }

        private ObservableCollection<ushort> _allHBins;
        public ObservableCollection<ushort> AllHBins {
            get { return _allHBins; }
            set { SetProperty(ref _allHBins, value); }
        }

        private ObservableCollection<ushort> _enabledHBins;
        public ObservableCollection<ushort> EnabledHBins {
            get { return _enabledHBins; }
            set { SetProperty(ref _enabledHBins, value); }
        }

        private ObservableCollection<ushort> _allSBins;
        public ObservableCollection<ushort> AllSBins {
            get { return _allSBins; }
            set { SetProperty(ref _allSBins, value); }
        }

        private ObservableCollection<string> _allSBinsFullName;
        public ObservableCollection<string> AllSBinsFullName
        {
            get { return _allSBinsFullName; }
            set { SetProperty(ref _allSBinsFullName, value); }
        }

        private ObservableCollection<ushort> _enabledSBins;
        public ObservableCollection<ushort> EnabledSBins {
            get { return _enabledSBins; }
            set { SetProperty(ref _enabledSBins, value); }
        }

        private IEnumerable<Item> _items;
        public IEnumerable<Item> Items {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private string _partFilterLowLimit;
        public string PartFilterLowLimit {
            get { return _partFilterLowLimit; }
            set { SetProperty(ref _partFilterLowLimit, value); }
        }

        private string _partFilterHighLimit;
        public string PartFilterHighLimit {
            get { return _partFilterHighLimit; }
            set { SetProperty(ref _partFilterHighLimit, value); }
        }

        private string _syncItemInfo;
        public string SyncItemInfo {
            get { return _syncItemInfo; }
            set { SetProperty(ref _syncItemInfo, value); }
        }

        private ObservableCollection<ItemFilter> _itemFilters = new ObservableCollection<ItemFilter>();
        public ObservableCollection<ItemFilter> ItemFilters {
            get { return _itemFilters; }
            set { SetProperty(ref _itemFilters, value); }
        }

        private string _maskEnableChips;
        public string MaskEnableChips {
            get { return _maskEnableChips; }
            set { SetProperty(ref _maskEnableChips, value); }
        }

        private string _maskEnableCords;
        public string MaskEnableCords {
            get { return _maskEnableCords; }
            set { SetProperty(ref _maskEnableCords, value); }
        }

        //private bool _ifmaskDuplicateChips;
        //public bool IfmaskDuplicateChips {
        //    get { return _ifmaskDuplicateChips; }
        //    set { SetProperty(ref _ifmaskDuplicateChips, value); }
        //}

        private bool _maskOrEnableIds;
        public bool MaskOrEnableIds {
            get { return _maskOrEnableIds; }
            set { SetProperty(ref _maskOrEnableIds, value); }
        }

        private bool _maskOrEnableCords;
        public bool MaskOrEnableCords {
            get { return _maskOrEnableCords; }
            set { SetProperty(ref _maskOrEnableCords, value); }
        }

        //private DuplicateSelectMode _duplicateSelectMode;
        //public DuplicateSelectMode DuplicateSelectMode {
        //    get { return _duplicateSelectMode; }
        //    set { SetProperty(ref _duplicateSelectMode, value); }
        //}

        //private DuplicateJudgeMode _judgeMode;
        //public DuplicateJudgeMode JudgeMode {
        //    get { return _judgeMode; }
        //    set { SetProperty(ref _judgeMode, value); }
        //}

        private bool _enFilterPanel=false;
        public bool EnFilterPanel {
            get { return _enFilterPanel; }
            set { SetProperty(ref _enFilterPanel, value); }
        }

        string _filePath;
        int _filterId;
        FilterSetup _filter;

        public DataFilterViewModel(IEventAggregator ea) {
            _ea = ea;
            UpdateFilter();
            _ea.GetEvent<Event_SubDataSelected>().Subscribe(UpdateDisplay);
            _ea.GetEvent<Event_DataSelected>().Subscribe(UpdateFilter);
            InitUiCtr();
        }

        private void UpdateDisplay(SubData selectedData) {

            var dataAcquire = StdDB.GetDataAcquire(selectedData.StdFilePath);
            _filePath = selectedData.StdFilePath;
            _filterId = selectedData.FilterId;

            _filter = dataAcquire.GetFilterSetup(selectedData.FilterId);

            Items = dataAcquire.GetFilteredItemStatistic(selectedData.FilterId);
            PartFilterLowLimit = "";
            PartFilterHighLimit = "";
            SyncItemInfo = "";

            AllSites = new ObservableCollection<byte>(dataAcquire.GetSites().OrderBy(x => x));
            EnabledSites = new ObservableCollection<byte>(AllSites.Except(_filter.MaskSites).OrderBy(x => x));
            AllHBins = new ObservableCollection<ushort>(dataAcquire.GetHardBins().OrderBy(x => x));
            EnabledHBins = new ObservableCollection<ushort>(AllHBins.Except(_filter.MaskHardBins).OrderBy(x => x));
            AllSBins = new ObservableCollection<ushort>(dataAcquire.GetSoftBins().OrderBy(x => x));

            var sT = dataAcquire.GetSBinInfo();
            List<string> listSbin = new List<string>();
            foreach(var key in sT.Keys.OrderBy(x=>x))
                listSbin.Add(key.ToString() + "-" + sT[key].Item1);
            AllSBinsFullName = new ObservableCollection<string>(listSbin);
            
            EnabledSBins = new ObservableCollection<ushort>(AllSBins.Except(_filter.MaskSoftBins).OrderBy(x => x));

            //IfmaskDuplicateChips = _filter.IfmaskDuplicateChips;
            //DuplicateSelectMode = _filter.DuplicateSelectMode;
            //JudgeMode = _filter.DuplicateJudgeMode;

            StringBuilder sb = new StringBuilder();
            foreach (var v in _filter.MaskChips) {
                sb.Append($"{v},");
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            MaskEnableChips = sb.ToString();

            sb.Clear();
            foreach (var v in _filter.MaskCords) {
                sb.Append($"{v.Item1},{v.Item2};");
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            MaskEnableCords = sb.ToString();


            MaskOrEnableIds = _filter.IfMaskOrEnableIds;
            MaskOrEnableCords = _filter.IfMaskOrEnableCords;

            ItemFilters.Clear();
            ItemFilters.AddRange(_filter.ItemFilters);
            EnFilterPanel = true;
        }

        public void UpdateFilter(string path) {
            UpdateFilter();
        }

        public void UpdateFilter() {
            EnFilterPanel = false;

            _filePath = null;
            _filter = null;

            AllSites = null;
            EnabledSites = null;
            AllHBins = null;
            EnabledHBins = null;
            AllSBins = null;
            EnabledSBins = null;

            //IfmaskDuplicateChips = false;
            //DuplicateSelectMode = DuplicateSelectMode.First;
            //JudgeMode = DuplicateJudgeMode.ID;

            MaskEnableChips = "";
            MaskEnableCords = "";

            MaskOrEnableIds = false;
            MaskOrEnableCords = false;

            Items = Enumerable.Empty<Item>();
            PartFilterLowLimit = "";
            PartFilterHighLimit = "";
            SyncItemInfo = "";

            ItemFilters.Clear();
        }

        private string TestItemStatistic(float loRange, float hiRange, IEnumerable<float> data) {
            int loFail = 0;
            int hiFail = 0;
            int naFail = 0;
            int negInf = 0;
            int posInf = 0;
            int pass = 0;
            int validatedCnt = 0;
            foreach(var v in data) {
                if (float.IsNaN(v)) {
                    naFail++;
                    continue;
                }
                if (float.IsPositiveInfinity(v)) {
                    posInf++;
                    continue;
                }
                if (float.IsNegativeInfinity(v)) {
                    negInf++;
                    continue;
                }
                validatedCnt++;
                if (v < loRange) {
                    loFail++;
                    continue;
                }
                if (v > hiRange) {
                    hiFail++;
                    continue;
                }
                pass++;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Total:{data.Count()}");
            sb.AppendLine($"Valid:{validatedCnt} NaN:{naFail} -∞:{negInf} +∞:{posInf}");
            if (validatedCnt > 0) {
                sb.AppendLine($"Pass:{pass} {(pass * 100.0 / validatedCnt).ToString("f3")}%");
                sb.AppendLine($"Lo Fail:{loFail} {(loFail*100.0/validatedCnt).ToString("f3")}%");
                sb.AppendLine($"Hi Fail:{hiFail} {(hiFail * 100.0 / validatedCnt).ToString("f3")}%");
            }

            return sb.ToString();
        }

        private string GetItemInfoString(string uid, ItemInfo info, ItemStatistic statistic) {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Lo Limt:{info.LoLimit} {info.Unit}  Hi Limt:{info.HiLimit} {info.Unit}");
            sb.AppendLine($"");

            return sb.ToString();
        }

        #region UI
        private string _curSelectedTN = "";

        private DelegateCommand<object> cmdSelectItem;
        public DelegateCommand<object> CmdSelectItem =>
            cmdSelectItem ?? (cmdSelectItem = new DelegateCommand<object>(ExecuteCmdSelectItem));

        void ExecuteCmdSelectItem(object parameter) {
            if(parameter is Item) {
                _curSelectedTN = ((Item)parameter).TestNumber;

                PartFilterLowLimit = float.NegativeInfinity.ToString();
                PartFilterHighLimit = float.PositiveInfinity.ToString();

                var da = StdDB.GetDataAcquire(_filePath);
                var statistic = da.GetFilteredStatistic(_filterId, _curSelectedTN);
                var info = da.GetTestInfo(_curSelectedTN);

                SyncItemInfo = GetItemInfoString(_curSelectedTN, info, statistic);
            }
        }

        private DelegateCommand partFilterApplyNegInfty;
        public DelegateCommand PartFilterApplyNegInfty =>
            partFilterApplyNegInfty ?? (partFilterApplyNegInfty = new DelegateCommand(ExecutePartFilterApplyNegInfty));

        void ExecutePartFilterApplyNegInfty() {
            PartFilterLowLimit = float.NegativeInfinity.ToString();
        }

        private DelegateCommand partFilterApplyPosInfty;
        public DelegateCommand PartFilterApplyPosInfty =>
            partFilterApplyPosInfty ?? (partFilterApplyPosInfty = new DelegateCommand(ExecutePartFilterApplyPosInfty));

        void ExecutePartFilterApplyPosInfty() {
            PartFilterHighLimit = float.PositiveInfinity.ToString();
        }

        private DelegateCommand partFilterTryLimit;
        public DelegateCommand PartFilterTryLimit =>
            partFilterTryLimit ?? (partFilterTryLimit = new DelegateCommand(ExecutePartFilterTryLimit));

        void ExecutePartFilterTryLimit() {
            try {
                var lr = float.Parse(PartFilterLowLimit);
                var hr = float.Parse(PartFilterHighLimit);
                if (lr > hr || _curSelectedTN == "") throw new Exception();

                var da = StdDB.GetDataAcquire(_filePath);
                var statistic = da.GetFilteredStatistic(_filterId, _curSelectedTN);
                var info = da.GetTestInfo(_curSelectedTN);

                SyncItemInfo = GetItemInfoString(_curSelectedTN, info, statistic) + TestItemStatistic(lr, hr,da.GetFilteredItemData(_curSelectedTN, _filterId));
            }
            catch {
                System.Windows.Forms.MessageBox.Show("Error Range or Exsist the item!");
                return;
            }

        }

        private DelegateCommand partFilterAddFilter;
        public DelegateCommand PartFilterAddFilter =>
            partFilterAddFilter ?? (partFilterAddFilter = new DelegateCommand(ExecutePartFilterAddFilter));

        void ExecutePartFilterAddFilter() {
            try {
                var lr = float.Parse(PartFilterLowLimit);
                var hr = float.Parse(PartFilterHighLimit);
                if (lr > hr || _curSelectedTN == "") throw new Exception();
                foreach(var v in ItemFilters){
                    if (v.TestNumber == _curSelectedTN) throw new Exception();
                }
                ItemFilters.Add(new ItemFilter(_curSelectedTN, lr, hr));
            }
            catch {
                System.Windows.Forms.MessageBox.Show("Error Range or Exsist the item!");
                return;
            }
        }

        private DelegateCommand<object> removeItemLimitFilter;
        public DelegateCommand<object> RemoveItemLimitFilter =>
            removeItemLimitFilter ?? (removeItemLimitFilter = new DelegateCommand<object>(ExecuteRemoveItemLimitFilter));

        void ExecuteRemoveItemLimitFilter(object e) {
            if(e is ItemFilter) {
                ItemFilters.Remove((ItemFilter)e);
            }
        }

        private DelegateCommand resetFilter;
        public DelegateCommand ResetFilter =>
             resetFilter ?? ( resetFilter = new DelegateCommand(ExecuteResetFilter));

        void ExecuteResetFilter() {
            if (_filter is null) {
                return;
            }
            var dataAcquire = StdDB.GetDataAcquire(_filePath);

            Items = dataAcquire.GetFilteredItemStatistic(_filterId);
            PartFilterLowLimit = "";
            PartFilterHighLimit = "";
            SyncItemInfo = "";

            AllSites = new ObservableCollection<byte>(dataAcquire.GetSites().OrderBy(x => x));
            EnabledSites = new ObservableCollection<byte>(AllSites);
            AllHBins = new ObservableCollection<ushort>(dataAcquire.GetHardBins().OrderBy(x => x));
            EnabledHBins = new ObservableCollection<ushort>(AllHBins);
            AllSBins = new ObservableCollection<ushort>(dataAcquire.GetSoftBins().OrderBy(x => x));

            EnabledSBins = new ObservableCollection<ushort>(AllSBins);

            //IfmaskDuplicateChips = _filter.IfmaskDuplicateChips;
            //DuplicateSelectMode = _filter.DuplicateSelectMode;
            //JudgeMode = _filter.DuplicateJudgeMode;

            MaskEnableChips = "";

            MaskEnableCords = "";

            MaskOrEnableIds = false;
            MaskOrEnableCords = false;

            ItemFilters.Clear();

            ExecuteApplyFilter();
        }

        private DelegateCommand applyFilter;
        public DelegateCommand ApplyFilter =>
            applyFilter ?? (applyFilter = new DelegateCommand(ExecuteApplyFilter));

        void ExecuteApplyFilter() {
            if (_filter is null) {
                return;
            }
            _ea.GetEvent<Event_Progress>().Publish(new Tuple<string, int>("Updating Filter", 50));

            //_filter.DuplicateSelectMode = DuplicateSelectMode;
            //_filter.DuplicateJudgeMode = JudgeMode;
            //_filter.IfmaskDuplicateChips = IfmaskDuplicateChips;
            _filter.IfMaskOrEnableIds = MaskOrEnableIds;
            _filter.IfMaskOrEnableCords = MaskOrEnableCords;

            _filter.MaskSites = AllSites.Except(EnabledSites).ToList();
            _filter.MaskHardBins = AllHBins.Except(EnabledHBins).ToList();
            _filter.MaskSoftBins = AllSBins.Except(EnabledSBins).ToList();
            _filter.MaskChips = ParseMaskEnableIds();
            _filter.MaskCords = ParseMaskEnableCords();

            _filter.ItemFilters.Clear();
            _filter.ItemFilters.AddRange(ItemFilters);

            var dataAcquire = StdDB.GetDataAcquire(_filePath);
            dataAcquire.UpdateFilter(_filterId, _filter);
            _ea.GetEvent<Event_FilterUpdated>().Publish(new SubData(_filePath, _filterId));
            _ea.GetEvent<Event_Log>().Publish("Updated Filter Done");
        }

        public DelegateCommand<ListBox> RemoveSite { get; private set; }
        public DelegateCommand<ListBox> AddSite { get; private set; }
        public DelegateCommand AddAllSites { get; private set; }
        public DelegateCommand<ListBox> AddSites { get; private set; }
        public DelegateCommand RemoveAllSites { get; private set; }
        public DelegateCommand<ListBox> RemoveSites { get; private set; }
        public DelegateCommand<ListBox> RemoveHBin { get; private set; }
        public DelegateCommand<ListBox> AddHBin { get; private set; }
        public DelegateCommand AddAllHBins { get; private set; }
        public DelegateCommand<ListBox> AddHBins { get; private set; }
        public DelegateCommand RemoveAllHBins { get; private set; }
        public DelegateCommand<ListBox> RemoveHBins { get; private set; }
        public DelegateCommand<ListBox> RemoveSBin { get; private set; }
        public DelegateCommand<ListBox> AddSBin { get; private set; }

        public DelegateCommand<ListBox> AddSBinFull { get; private set; }

        public DelegateCommand AddAllSBins { get; private set; }
        public DelegateCommand<ListBox> AddSBins { get; private set; }
        public DelegateCommand RemoveAllSBins { get; private set; }
        public DelegateCommand<ListBox> RemoveSBins { get; private set; }
        public DelegateCommand ClearIds { get; private set; }
        public DelegateCommand ClearCords { get; private set; }

        private void InitUiCtr() {

            RemoveSite = new DelegateCommand<ListBox>((e) => {
                var v = ((ListBox)(e));
                if (v.Items.Count > 0 && v.SelectedIndex >= 0)
                    EnabledSites.RemoveAt(v.SelectedIndex);
            });
            AddSite = new DelegateCommand<ListBox>((e) => {
                var v = ((ListBox)(e));
                if (v.SelectedIndex >= 0 && !EnabledSites.Contains((byte)v.SelectedItem))
                    EnabledSites.Add((byte)v.SelectedItem);

                EnabledSites.OrderBy(x => x);
            });
            AddAllSites = new DelegateCommand(() => {
                EnabledSites.Clear();
                foreach (var v in AllSites)
                    EnabledSites.Add(v);
            });
            AddSites = new DelegateCommand<ListBox>((e) => {
                if (e.SelectedItems.Count >= 0) {
                    foreach (var v in e.SelectedItems)
                        if (!EnabledSites.Contains((byte)v))
                            EnabledSites.Add((byte)v);
                }

                EnabledSites.OrderBy(x => x);
            });
            RemoveAllSites = new DelegateCommand(() => {
                EnabledSites.Clear();
            });
            RemoveSites = new DelegateCommand<ListBox>((e) => {
                if (e.SelectedItems.Count >= 0) {
                    for(int i= EnabledSites.Count-1; i>=0; i--) {
                        if (e.SelectedItems.Contains(EnabledSites[i])) EnabledSites.RemoveAt(i);
                    }
                }
            });

            RemoveHBin = new DelegateCommand<ListBox>((e) => {
                var v = ((ListBox)(e));
                if (v.Items.Count > 0 && v.SelectedIndex >= 0)
                    EnabledHBins.RemoveAt(v.SelectedIndex);
            });
            AddHBin = new DelegateCommand<ListBox>((e) => {
                var v = ((ListBox)(e));
                if (v.SelectedIndex >= 0 && !EnabledHBins.Contains((ushort)v.SelectedItem))
                    EnabledHBins.Add((ushort)v.SelectedItem);

                EnabledHBins.OrderBy(x => x);
            });
            AddAllHBins = new DelegateCommand(() => {
                EnabledHBins.Clear();
                foreach (var v in AllHBins)
                    EnabledHBins.Add(v);
            });
            AddHBins = new DelegateCommand<ListBox>((e) => {
                if (e.SelectedItems.Count >= 0) {
                    foreach (var v in e.SelectedItems)
                        if (!EnabledHBins.Contains((ushort)v))
                            EnabledHBins.Add((ushort)v);
                }

                EnabledHBins.OrderBy(x => x);
            });
            RemoveAllHBins = new DelegateCommand(() => {
                EnabledHBins.Clear();
            });
            RemoveHBins = new DelegateCommand<ListBox>((e) => {
                if (e.SelectedItems.Count >= 0) {
                    for (int i = EnabledHBins.Count - 1; i >= 0; i--) {
                        if (e.SelectedItems.Contains(EnabledHBins[i])) EnabledHBins.RemoveAt(i);
                    }
                }
            });

            RemoveSBin = new DelegateCommand<ListBox>((e) => {
                var v = ((ListBox)(e));
                if (v.Items.Count > 0 && v.SelectedIndex >= 0)
                    EnabledSBins.RemoveAt(v.SelectedIndex);
            });
            AddSBin = new DelegateCommand<ListBox>((e) => {
                var v = ((ListBox)(e));
                if (v.SelectedIndex >= 0 && !EnabledSBins.Contains((ushort)v.SelectedItem))
                    EnabledSBins.Add((ushort)v.SelectedItem);

                EnabledSBins.OrderBy(x => x);
            });

            AddSBinFull = new DelegateCommand<ListBox>((e) => {
                var v = ((ListBox)(e));
                string selectFullItem = v.SelectedItem.ToString().Split('-')[0];
                ushort i = (ushort) int.Parse(selectFullItem);
                if (v.SelectedIndex >= 0 && !EnabledSBins.Contains(i))
                    EnabledSBins.Add(i);

                EnabledSBins.OrderBy(x => x);
            });

            AddAllSBins = new DelegateCommand(() => {
                EnabledSBins.Clear();
                foreach (var v in AllSBins)
                    EnabledSBins.Add(v);
            });
            AddSBins = new DelegateCommand<ListBox>((e) => {
                if (e.SelectedItems.Count >= 0) {
                    foreach (var v in e.SelectedItems)
                        if (!EnabledSBins.Contains((ushort)v))
                            EnabledSBins.Add((ushort)v);
                }

                EnabledSBins.OrderBy(x => x);
            });
            RemoveAllSBins = new DelegateCommand(() => {
                EnabledSBins.Clear();
            });
            RemoveSBins = new DelegateCommand<ListBox>((e) => {
                if (e.SelectedItems.Count >= 0) {
                    for (int i = EnabledSBins.Count - 1; i >= 0; i--) {
                        if (e.SelectedItems.Contains(EnabledSBins[i])) EnabledSBins.RemoveAt(i);
                    }
                }
            });

            ClearIds = new DelegateCommand(() => {
                MaskEnableChips = "";
            });

            ClearCords = new DelegateCommand(() => {
                MaskEnableCords = "";
            });

        }

        private List<int> ParseMaskEnableIds() {
            var da = StdDB.GetDataAcquire(_filePath);
            var cnt = da.ChipsCount;

            List<int> rst = new List<int>();
            var ss = MaskEnableChips.Split(';');
            foreach (string s in ss) {
                try {
                    if (s.Contains("-")) {
                        var m = System.Text.RegularExpressions.Regex.Match(s, @"\s*(\d*)\s*-\s*(\d*)\s*");
                        if (!m.Success) continue;
                        var pre = m.Groups[1].Length==0 ? 0 : int.Parse(m.Groups[1].Value);
                        var post = m.Groups[2].Length == 0 ? cnt-1 : int.Parse(m.Groups[2].Value);

                        for(int i = pre; i<=post; i++) {
                            if (i < cnt) rst.Add(i);
                        }

                    } else {
                        var v = int.Parse(s.Trim());
                        if(v>=0 && v< cnt) rst.Add(v); 
                    }
                } catch { 
                    continue; 
                }
            }
            return rst;
        }
        private List<Tuple<short, short>> ParseMaskEnableCords() {
            List<Tuple<short, short>> rst = new List<Tuple<short, short>>();
            var ss = MaskEnableCords.Split(';');
            foreach (string s in ss) {
                var xy = s.Split(',');
                if (xy.Length != 2) continue;
                try {
                    short x = short.Parse(xy[0].Trim());
                    short y = short.Parse(xy[1].Trim());
                    rst.Add(new Tuple<short, short>(x, y));
                } catch { continue; }
            }

            return rst;
        }

        private DelegateCommand _cmdGetDupCords;
        public DelegateCommand CmdGetDupCords =>
            _cmdGetDupCords ?? (_cmdGetDupCords = new DelegateCommand(ExecuteCmdGetDupCords));

        void ExecuteCmdGetDupCords() {

            var da = StdDB.GetDataAcquire(_filePath);
            var allCords = da.GetAllCords();
            var dups = allCords.GroupBy(x => $"{x.Item1},{x.Item2}")
                .Where(g => g.Count() > 1)
                .Select(y => y.Key);

            StringBuilder sb = new StringBuilder();
            foreach(var v in dups) {
                sb.Append(v);
                sb.Append(";");
            }

            MaskEnableCords = sb.ToString();
        }

        private DelegateCommand _cmdGetFreshIdx;
        public DelegateCommand CmdGetFreshIdx =>
            _cmdGetFreshIdx ?? (_cmdGetFreshIdx = new DelegateCommand(ExecuteCmdGetFreshIdx));

        void ExecuteCmdGetFreshIdx() {
            var da = StdDB.GetDataAcquire(_filePath);
            var allCords = da.GetAllCordsAndIdx();
            var dups = allCords.GroupBy(x => (x.Item1,x.Item2))
                .Where(g => g.Count() > 1)
                .Select(y => y);

            StringBuilder sb = new StringBuilder();
            foreach (var vv in dups) {
                var idx = (from v in vv
                 select v.Item3).Min();
                sb.Append(idx);
                sb.Append(";");
            }
            MaskEnableChips = sb.ToString();
        }

        private DelegateCommand _cmdGetRtIdx;
        public DelegateCommand CmdGetRtIdx=>
            _cmdGetRtIdx ?? (_cmdGetRtIdx = new DelegateCommand(ExecuteCmdGetRtIdx));

        void ExecuteCmdGetRtIdx() {
            var da = StdDB.GetDataAcquire(_filePath);
            var allCords = da.GetAllCordsAndIdx();
            var dups = allCords.GroupBy(x => (x.Item1, x.Item2))
                .Where(g => g.Count() > 1)
                .Select(y => y);

            StringBuilder sb = new StringBuilder();
            foreach (var vv in dups) {
                var idx = (from v in vv
                           select v.Item3).Max();
                sb.Append(idx);
                sb.Append(";");
            }
            MaskEnableChips = sb.ToString();
        }

        #endregion


    }
}
