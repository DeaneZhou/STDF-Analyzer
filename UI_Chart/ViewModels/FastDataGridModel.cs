﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DataContainer;
using FastWpfGrid;

namespace UI_Chart.ViewModels {
    public class FastDataGridModel : FastGridModelBase{
        private IDataAcquire _da;
        private SubData _subData;

        private Color? _cellColor;

        private HashSet<int> _frozenCols = new HashSet<int>();
        private HashSet<int> _frozenRows = new HashSet<int>();

        public FastDataGridModel(SubData subData) {
            _subData = subData;
            _da = StdDB.GetDataAcquire(subData.StdFilePath);

            //_frozenRows.Add(0);
            //_frozenRows.Add(1);
            //_frozenRows.Add(2);
            //_frozenRows.Add(3);
            //_frozenRows.Add(4);
            //_frozenRows.Add(5);

            _frozenCols.Add(0);
            _frozenCols.Add(1);
        }

        public override HashSet<int> GetFrozenColumns(IFastGridView view) {
            return _frozenCols;
        }

        public override HashSet<int> GetFrozenRows(IFastGridView view) {
            return _frozenRows;
        }

        public override string GetColumnHeaderText(int column) {
            //return (column+1).ToString();

            if (column == 0) {
                return string.Empty;
            } else if (column == 1) {
                return $"Index\nCord\nTime\nHBin\nSBin\nSite";
            } else if (column > 1) {
                var idx = _da.GetFilteredPartIndex(_subData.FilterId).ElementAt(column - 2);
                return $"{idx.ToString()}\n{_da.GetWaferCord(idx)}\n{_da.GetTestTime(idx).ToString()}\n{_da.GetHardBin(idx).ToString()}\n{_da.GetSoftBin(idx).ToString()}\n{_da.GetSite(idx).ToString()}";
            }
            return string.Empty;
        }

        public override string GetRowHeaderText(int row) {
            return string.Empty;
        }

        public override int ColumnCount {
            get { return _da.GetFilteredChipsCount(_subData.FilterId) + 2; }
        }

        public override int RowCount {
            get { return _da.GetTestIDs().Count(); }
        }

        public override Color? FontColor {
            get { return _cellColor; }
        }

        public override string GetCellText(int row, int column) {
            _cellColor = null;

            if (column == 0) {
                return (row+1).ToString();
            }else if (column == 1) {
                return _da.GetTestIDs().ElementAt(row);
            } else if(column > 1){
                var idx = _da.GetFilteredPartIndex(_subData.FilterId).ElementAt(column - 2);
                
                var uid = _da.GetTestIDs().ElementAt(row);
                var val = _da.GetItemData(uid, idx);
                var limit = _da.GetTestInfo(uid);

                if(limit.LoLimit.HasValue && val < limit.LoLimit)
                    _cellColor = Colors.Blue;
                else if(limit.HiLimit.HasValue && val > limit.HiLimit)
                    _cellColor = Colors.Red;

                return val.ToString();
            }
            return string.Empty;
        }

        //public override void SetCellText(int row, int column, string value) {
        //    var key = Tuple.Create(row, column);
        //    _editedCells[key] = value;
        //}

        //public override IFastGridCell GetGridHeader(IFastGridView view) {
        //    var impl = new FastGridCellImpl();
        //    var btn = impl.AddImageBlock(view.IsTransposed ? "/Images/flip_horizontal_small.png" : "/Images/flip_vertical_small.png");
        //    btn.CommandParameter = FastWpfGrid.FastGridControl.ToggleTransposedCommand;
        //    btn.ToolTip = "Swap rows and columns";
        //    impl.AddImageBlock("/Images/foreign_keysmall.png").CommandParameter = "FK";
        //    impl.AddImageBlock("/Images/primary_keysmall.png").CommandParameter = "PK";
        //    return impl;
        //}

        //public override void HandleCommand(IFastGridView view, FastGridCellAddress address, object commandParameter, ref bool handled) {
        //    //base.HandleCommand(view, address, commandParameter, ref handled);
        //    //if (commandParameter is string) MessageBox.Show(commandParameter.ToString());
        //}

        //public override int? SelectedRowCountLimit {
        //    get { return 100; }
        //}

        //public override void HandleSelectionCommand(IFastGridView view, string command) {
        //    //MessageBox.Show(command);
        //}


    }
}
