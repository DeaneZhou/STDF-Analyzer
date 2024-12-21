using DataContainer;
using Microsoft.Win32;
using Prism.Events;
using Prism.Regions;
using ScottPlot.Plottable;
using SillyMonkey.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI_Chart.Views {
    /// <summary>
    /// Interaction logic for Trend
    /// </summary>
    public partial class Trend : UserControl, INavigationAware {
        public Trend(IRegionManager regionManager, IEventAggregator ea) {
            InitializeComponent();

            _regionManager = regionManager;
            _ea = ea;

            trendChart.RightClicked -= trendChart.DefaultRightClickEvent;
            trendChart.Configuration.DoubleClickBenchmark = false;
            trendChart.Configuration.LockVerticalAxis = true;
            

            histoChart.RightClicked -= histoChart.DefaultRightClickEvent;
            histoChart.Configuration.DoubleClickBenchmark = false;
            histoChart.Configuration.LockVerticalAxis = true;

            histoChart.Configuration.LeftClickDragPan = false;
            histoChart.Configuration.ScrollWheelZoom = false;
            histoChart.Configuration.RightClickDragZoom = false;

            histoChart.MouseDown += HistoChart_MouseDown;
            histoChart.MouseLeftButtonDown += HistoChart_MouseLeftButtonDown;
            histoChart.MouseMove += HistoChart_MouseMove;
            histoChart.MouseLeftButtonUp += HistoChart_MouseLeftButtonUp;
            histoChart.RightClicked += HistoChart_MouseRightButtonClicked;


            switch (SA.HistogramChartAxis){
                case ChartAxisType.Sigma:
                    radioSigmaHisto.IsChecked = true;
                    break;
                case ChartAxisType.MinMax:
                    radioMinMaxHisto.IsChecked = true;
                    break;
                case ChartAxisType.Limit:
                    radioLimitHisto.IsChecked = true;
                    break;
            }

            switch (SA.HistogramChartAxisSigmaRange) {
                case SigmaRangeType.Sigma6:
                    comboboxSigmaHisto.SelectedIndex = 0;
                    break;
                case SigmaRangeType.Sigma5:
                    comboboxSigmaHisto.SelectedIndex = 1;
                    break;
                case SigmaRangeType.Sigma4:
                    comboboxSigmaHisto.SelectedIndex = 2;
                    break;
                case SigmaRangeType.Sigma3:
                    comboboxSigmaHisto.SelectedIndex = 3;
                    break;
                case SigmaRangeType.Sigma2:
                    comboboxSigmaHisto.SelectedIndex = 4;
                    break;
                case SigmaRangeType.Sigma1:
                    comboboxSigmaHisto.SelectedIndex = 5;
                    break;
            }

            toggleOutlierHisto.IsChecked = SA.HistogramEnableOutlierFilter;

            switch (SA.HistogramOutlierFilterRange) {
                case SigmaRangeType.Sigma6:
                    comboboxOutlierSigmaHisto.SelectedIndex = 0;
                    break;
                case SigmaRangeType.Sigma5:
                    comboboxOutlierSigmaHisto.SelectedIndex = 1;
                    break;
                case SigmaRangeType.Sigma4:
                    comboboxOutlierSigmaHisto.SelectedIndex = 2;
                    break;
                case SigmaRangeType.Sigma3:
                    comboboxOutlierSigmaHisto.SelectedIndex = 3;
                    break;
                case SigmaRangeType.Sigma2:
                    comboboxOutlierSigmaHisto.SelectedIndex = 4;
                    break;
                case SigmaRangeType.Sigma1:
                    comboboxOutlierSigmaHisto.SelectedIndex = 5;
                    break;
            }

            AxisLimitHisto.IsChecked = SA.HistogramEnableLimitLine;
            AxisMinMaxHisto.IsChecked = SA.HistogramEnableMinMaxLine;
            AxisMeanHisto.IsChecked = SA.HistogramEnableMeanLine;
            AxisMedianHisto.IsChecked = SA.HistogramEnableMedianLine;
            AxisSigma6Histo.IsChecked = SA.HistogramEnableSigma6Line;
            AxisSigma3Histo.IsChecked = SA.HistogramEnableSigma3Line;


            switch (SA.TrendChartAxis) {
                case ChartAxisType.Sigma:
                    radioSigma.IsChecked = true;
                    break;
                case ChartAxisType.MinMax:
                    radioMinMax.IsChecked = true;
                    break;
                case ChartAxisType.Limit:
                    radioLimit.IsChecked = true;
                    break;
            }

            switch (SA.TrendChartAxisSigmaRange) {
                case SigmaRangeType.Sigma6:
                    comboboxSigma.SelectedIndex = 0;
                    break;
                case SigmaRangeType.Sigma5:
                    comboboxSigma.SelectedIndex = 1;
                    break;
                case SigmaRangeType.Sigma4:
                    comboboxSigma.SelectedIndex = 2;
                    break;
                case SigmaRangeType.Sigma3:
                    comboboxSigma.SelectedIndex = 3;
                    break;
                case SigmaRangeType.Sigma2:
                    comboboxSigma.SelectedIndex = 4;
                    break;
                case SigmaRangeType.Sigma1:
                    comboboxSigma.SelectedIndex = 5;
                    break;
            }

            toggleOutlierTrend.IsChecked = SA.TrendEnableOutlierFilter;

            switch (SA.TrendOutlierFilterRange) {
                case SigmaRangeType.Sigma6:
                    comboboxOutlierSigma.SelectedIndex = 0;
                    break;
                case SigmaRangeType.Sigma5:
                    comboboxOutlierSigma.SelectedIndex = 1;
                    break;
                case SigmaRangeType.Sigma4:
                    comboboxOutlierSigma.SelectedIndex = 2;
                    break;
                case SigmaRangeType.Sigma3:
                    comboboxOutlierSigma.SelectedIndex = 3;
                    break;
                case SigmaRangeType.Sigma2:
                    comboboxOutlierSigma.SelectedIndex = 4;
                    break;
                case SigmaRangeType.Sigma1:
                    comboboxOutlierSigma.SelectedIndex = 5;
                    break;
            }

            AxisLimitTrend.IsChecked = SA.TrendEnableLimitLine;
            AxisMinMaxTrend.IsChecked = SA.TrendEnableMinMaxLine;
            AxisMeanTrend.IsChecked = SA.TrendEnableMeanLine;
            AxisMedianTrend.IsChecked = SA.TrendEnableMedianLine;
            AxisSigma6Trend.IsChecked = SA.TrendEnableSigma6Line;
            AxisSigma3Trend.IsChecked = SA.TrendEnableSigma3Line;

            sitesChoosed = new List<int>();
            AllSites = new ObservableCollection<int>();
            IsMouseDown = false;

            //   var da = StdDB.GetDataAcquire(_subData.StdFilePath);
        }

        private void HistoChart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                IsZoom = false;
                toggleOutlierHisto.IsChecked = false;
                UpdateHisto();
                histoChart.Refresh();
            }
        }

        private int zoomTime = 1;
        private bool IsZoom = false;
        float zoomStartX = 0.0f;
        float zoomEndX = 0.0f;

        private void HistoChart_MouseRightButtonClicked(object sender, EventArgs e)
        {
            if (!IsZoom || CurrentParamChanged)
                return; // Not in Zoom, so return directly.

            if (zoomTime < 10) // Only allow zoom out 10 times.
                zoomTime++;
            else
                IsZoom = false;

            float ov = Math.Abs(zoomEndX - zoomStartX) * 0.25f * zoomTime;

            if (IsZoom)
                if (zoomStartX < zoomEndX)
                    UpdateHistoSeriesForZoom(zoomStartX - ov, zoomEndX + ov);
                else
                    UpdateHistoSeriesForZoom(zoomEndX - ov, zoomEndX + ov);
        }

        private void HistoChart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (poly != null)
                histoChart.Plot.Remove(poly);
            bool validMove = false;

            validMove =
                Math.Abs(startPointInPixcel[0] - endPointInPixcel[0])
                > 20
            /*&& Math.Abs(startPointInPixcel[1] - endPointInPixcel[1]) > 10*/;

            if (IsMouseDown && validMove)
            {
                zoomStartX = (float)startPoint[0];
                zoomEndX = (float)endPoint[0];

                if (zoomStartX < zoomEndX)
                    UpdateHistoSeriesForZoom(zoomStartX, zoomEndX);
                else
                    UpdateHistoSeriesForZoom(zoomEndX, zoomStartX);
                zoomTime = 1;
                IsZoom = true;
                CurrentParamChanged = false;
            }
            IsMouseDown = false;
            //histoChart.Refresh();
        }
        Polygon poly = null;

        private void HistoChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                int pixelX = (int)e.MouseDevice.GetPosition(histoChart).X;
                int pixelY = (int)e.MouseDevice.GetPosition(histoChart).Y;

                endPointInPixcel[0] = pixelX;
                endPointInPixcel[1] = pixelY;

                //var Point = e.GetPosition(histoChart);
                //(double coordinateRealX, double coordinateRealY) mouseCoordinates =
                //    histoChart.Plot.GetCoordinate((float)Point.X, (float)Point.Y);

                (double coordinateX, double coordinateY) = histoChart.GetMouseCoordinates();

                endPoint[0] = coordinateX;
                endPoint[1] = coordinateY;

                double[] startPointXs = new double[4];
                double[] endPointXs = new double[4];

                // x1, y1; x2, y2
                startPointXs[0] = startPoint[0]; // x1, y2
                startPointXs[1] = startPoint[0]; // x1, y1
                startPointXs[2] = endPoint[0]; // x2, y1
                startPointXs[3] = endPoint[0]; // x2, y2

                endPointXs[0] = endPoint[1];
                endPointXs[1] = startPoint[1];
                endPointXs[2] = startPoint[1];
                endPointXs[3] = endPoint[1];
                if (poly != null)
                    histoChart.Plot.Remove(poly);
                poly = histoChart.Plot.AddPolygon(
                    startPointXs,
                    endPointXs,
                    Color.Transparent,
                    1,
                    Color.Red
                );

                histoChart.Refresh();
            }
        }

        //        string txtX, txtY;
        double[] startPoint = new double[2];
        double[] endPoint = new double[2];

        int[] startPointInPixcel = new int[2];
        int[] endPointInPixcel = new int[2];

        private void HistoChart_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            if (true)
            {
                int pixelX = (int)e.MouseDevice.GetPosition(histoChart).X;
                int pixelY = (int)e.MouseDevice.GetPosition(histoChart).Y;

                startPointInPixcel[0] = pixelX;
                startPointInPixcel[1] = pixelY;

                (double coordinateX, double coordinateY) = histoChart.GetMouseCoordinates();

                startPoint[0] = coordinateX;
                startPoint[1] = coordinateY;
            }
        }
        IRegionManager _regionManager;
        IEventAggregator _ea;

        SubData _subData;
        List<string> _selectedIds;
        public List<string> SelectedIds {
            get { return _selectedIds; }
        }

        private int _deviceCount, _ubound;

        private float _allsigmaLowTrend, _allsigmaHighTrend;
        private float _allsigmaLowHisto, _allsigmaHighHisto;

        public ObservableCollection<int> AllSites { get; set; } // 20241122
        private List<int> sitesChoosed;

        private bool IsMouseDown = false;

        #region Binding_prop

        private float _lowLimit = float.NegativeInfinity;

        private float _highLimit = float.PositiveInfinity;

        private bool _ifShowLegendCheckBox = false;

        private string _itemTitleTrend;

        private string _itemTitleHisto;

        #endregion

        #region line
        private float _minTrend;
        private float _maxTrend;
        private float _meanTrend;
        private float _medianTrend;
        private float _sigma6LTrend;
        private float _sigma6HTrend;
        private float _sigma3LTrend;
        private float _sigma3HTrend;

        private float _minHisto;
        private float _maxHisto;
        private float _meanHisto;
        private float _medianHisto;
        private float _sigma6LHisto;
        private float _sigma6HHisto;
        private float _sigma3LHisto;
        private float _sigma3HHisto;
        
        private HLine lineMin_Trend;
        private HLine lineMax_Trend;
        private HLine lineLLimit_Trend;
        private HLine lineHLimit_Trend;
        private HLine lineMean_Trend;
        private HLine lineMedian_Trend;
        private HLine lineLSigma6_Trend;
        private HLine lineHSigma6_Trend;
        private HLine lineLSigma3_Trend;
        private HLine lineHSigma3_Trend;

        private VLine lineMin_Histo;
        private VLine lineMax_Histo;
        private VLine lineLLimit_Histo;
        private VLine lineHLimit_Histo;
        private VLine lineMean_Histo;
        private VLine lineMedian_Histo;
        private VLine lineLSigma6_Histo;
        private VLine lineHSigma6_Histo;
        private VLine lineLSigma3_Histo;
        private VLine lineHSigma3_Histo;
        #endregion

        void UpdateItems(Tuple<SubData, List<string>> para) {
            if (!para.Item1.Equals(_subData)) return;
            if (para.Item2 == null || para.Item2.Count == 0) return;

            _selectedIds.Clear();
            _selectedIds.AddRange(para.Item2);

            if (_selectedIds.Count == 1) {
                toggleSplitSite.IsEnabled = true;
            } else {
                toggleSplitSite.IsEnabled = false;
            }

            UpdateData();
        }

        bool _dataValid;

        int SigmaByIdx(int idx) {
            return 6 - idx;
        }

        bool isInvalid(float f) {
            return float.IsNaN(f) || float.IsInfinity(f);
        }

        void ClearChart() {
            //clear chart
            trendChart.Plot.Clear();
            trendChart.Refresh();

            histoChart.Plot.Clear();
            histoChart.Refresh();
        }

        //get raw data
        private bool CurrentParamChanged = true; //20241122
        void UpdateData() {
            if (_selectedIds == null || _selectedIds.Count == 0) {
                _dataValid = false;
                ClearChart();
                return;
            } else {
                _dataValid = true;
            }
            var da = StdDB.GetDataAcquire(_subData.StdFilePath);
			// 20241122
            if (AllSites.Count == 0)
            {
                foreach (var s in da.GetSites())
                    AllSites.Add(s);
            }
            CurrentParamChanged = true;

            var idInfo = da.GetTestInfo(_selectedIds[0]);
            _lowLimit = idInfo.LoLimit ?? float.NegativeInfinity;
            _highLimit = idInfo.HiLimit ?? float.PositiveInfinity;
            if (_selectedIds.Count == 1) {
                if (toggleSplitSite.IsEnabled && toggleSplitSite.IsChecked.Value)
                    _ifShowLegendCheckBox = true;
                else
                    _ifShowLegendCheckBox = false;
            } else {
                _ifShowLegendCheckBox = true;
            }
            
            var itemStat = da.GetFilteredStatistic(_subData.FilterId, _selectedIds[0]);

            if (isInvalid(itemStat.MeanValue))
                return;
            _deviceCount = da.GetFilteredChipsCount(_subData.FilterId);
            if (_deviceCount == 0) {
                _ubound = 1;
                _dataValid = false;
                ClearChart();
                return;
            } 

            UpdateTrend();

            UpdateHisto();
        }

        void UpdateFilter(SubData subData) {
            if (subData.Equals(_subData)) {
                UpdateData();
            }
        }

        (double[], double[]) GetPatialSeries(int[] part, float[] data) {
            List<double> xs = new List<double>(part.Length);
            List<double> ys = new List<double>(part.Length);

            for(int i=0; i< part.Length; i++) {
                if (!isInvalid(data[i])) {
                    xs.Add(part[i]);
                    ys.Add(data[i]);
                } 
            }
            return (xs.ToArray(), ys.ToArray());
        }

        void UpdateTrend() {
            if (!_dataValid) return;
            trendChart.Plot.Clear();

            var da = StdDB.GetDataAcquire(_subData.StdFilePath);

            var xs = da.GetFilteredPartIndex(_subData.FilterId).ToArray();

            for (int i = 0; i < (_selectedIds.Count > 16 ? 16 : _selectedIds.Count); i++) {
                var data = da.GetFilteredItemData(_selectedIds[i], _subData.FilterId).ToArray();

                var s = GetPatialSeries(xs, data);

                var color = SA.GetColor(i);

				// 20241122
                if (s.Item1.Length == 0 || s.Item2.Length == 0)
                    continue;

                //trendChart.Plot.AddSignalXY(s.Item1, s.Item2, Color.FromArgb(color.A, color.R, color.G, color.B), _selectedIds[i]);
                trendChart.Plot.AddScatter(s.Item1, s.Item2, Color.FromArgb(color.A, color.R, color.G, color.B), lineWidth: 0,label:  _selectedIds[i]); 
            }

            _ubound = xs[xs.Length-1];

            for (int i = 0; i < (_selectedIds.Count > 16 ? 16 : _selectedIds.Count); i++) {
                var statistic_raw = da.GetFilteredStatistic(_subData.FilterId, _selectedIds[i]);
                ItemStatistic statistic;

                if (toggleOutlierTrend.IsChecked.Value) {
                    statistic = da.GetFilteredStatisticIgnoreOutlier(_subData.FilterId, _selectedIds[i], SigmaByIdx(comboboxOutlierSigma.SelectedIndex));
                } else {
                    statistic = statistic_raw;
                }

                if (i == 0) {
                    _minTrend = statistic.MinValue;
                    _maxTrend = statistic.MaxValue;

                    _allsigmaLowTrend = statistic.GetSigmaRangeLow(SigmaByIdx(comboboxSigma.SelectedIndex));
                    _allsigmaHighTrend = statistic.GetSigmaRangeHigh(SigmaByIdx(comboboxSigma.SelectedIndex));

                    if (_selectedIds.Count == 1) {
                        var info = da.GetTestInfo(_selectedIds[0]);
                        var failRate = (statistic.FailCount * 100.0 / statistic.ValidCount).ToString("f3") + "%";
                        _itemTitleTrend = $"{_selectedIds[0]}:{info.TestText}\nmean|{statistic.MeanValue:f3}  median|{statistic.MedianValue:f3}  cpk|{statistic.Cpk:f3}  σ|{statistic.Sigma:f3}  fail|{statistic.FailCount}/{statistic.ValidCount}={failRate}";
                    } else {
                        _itemTitleTrend = _selectedIds[0];
                    }

                    _meanTrend = statistic.MeanValue;
                    _medianTrend = statistic.MedianValue;
                    _sigma3LTrend = statistic.GetSigmaRangeLow(3);
                    _sigma3HTrend = statistic.GetSigmaRangeHigh(3);
                    _sigma6LTrend = statistic.GetSigmaRangeLow(6);
                    _sigma6HTrend = statistic.GetSigmaRangeHigh(6);

                } else {
                    _minTrend = statistic.MinValue < _minTrend ? statistic.MinValue : _minTrend;
                    _maxTrend = statistic.MaxValue > _maxTrend ? statistic.MaxValue : _maxTrend;

                    var sigmaLow = statistic.GetSigmaRangeLow(SigmaByIdx(comboboxSigma.SelectedIndex));
                    var sigmaHigh = statistic.GetSigmaRangeHigh(SigmaByIdx(comboboxSigma.SelectedIndex));

                    if (sigmaLow < _allsigmaLowTrend) _allsigmaLowTrend = sigmaLow;
                    if (sigmaHigh > _allsigmaHighTrend) _allsigmaHighTrend = sigmaHigh;

                    _itemTitleTrend += " & " + _selectedIds[i];
                }
            }
            trendChart.Plot.Title(_itemTitleTrend,true,null,12);
            trendChart.Plot.Legend(_ifShowLegendCheckBox, ScottPlot.Alignment.UpperRight);

            UpdateAxis_Trend();

            //set the y axix
            if (radioSigma.IsChecked.Value) {
                ExecuteCmdSelectAxisSigmaTrend();
            } else if (radioMinMax.IsChecked.Value) {
                ExecuteCmdSelectAxisMinMaxTrend();
            } else if (radioLimit.IsChecked.Value) {
                ExecuteCmdSelectAxisLimitTrend();
            } else {
                ExecuteCmdSelectAxisUserTrend();
            }
        }

        void UpdateHisto() {
            if (!_dataValid) return;
            var da = StdDB.GetDataAcquire(_subData.StdFilePath);

            for (int i = 0; i < (_selectedIds.Count > 16 ? 16 : _selectedIds.Count); i++) {
                var statistic_raw = da.GetFilteredStatistic(_subData.FilterId, _selectedIds[i]);
                ItemStatistic statistic;

                if (toggleOutlierHisto.IsChecked.Value) {
                    statistic = da.GetFilteredStatisticIgnoreOutlier(_subData.FilterId, _selectedIds[i], SigmaByIdx(comboboxOutlierSigmaHisto.SelectedIndex));
                } else {
                    statistic = statistic_raw;
                }

                if (i == 0) {
                    _minHisto = statistic.MinValue;
                    _maxHisto = statistic.MaxValue;

                    _allsigmaLowHisto = statistic.GetSigmaRangeLow(SigmaByIdx(comboboxSigmaHisto.SelectedIndex));
                    _allsigmaHighHisto = statistic.GetSigmaRangeHigh(SigmaByIdx(comboboxSigmaHisto.SelectedIndex));

                    if (_selectedIds.Count == 1) {
                        var info = da.GetTestInfo(_selectedIds[0]);
                        var failRate = (statistic.FailCount * 100.0 / statistic.ValidCount).ToString("f3") + "%";
                        _itemTitleHisto = $"{_selectedIds[0]}:{info.TestText}\nmean|{statistic.MeanValue:f3}  median|{statistic.MedianValue:f3}  cpk|{statistic.Cpk:f3}  σ|{statistic.Sigma:f3}  fail|{statistic.FailCount}/{statistic.ValidCount}={failRate}";
                    } else {
                        _itemTitleHisto = _selectedIds[0];
                    }

                    _meanHisto = statistic.MeanValue;
                    _medianHisto = statistic.MedianValue;
                    _sigma3LHisto = statistic.GetSigmaRangeLow(3);
                    _sigma3HHisto = statistic.GetSigmaRangeHigh(3);
                    _sigma6LHisto = statistic.GetSigmaRangeLow(6);
                    _sigma6HHisto = statistic.GetSigmaRangeHigh(6);


                } else {
                    _minHisto = statistic.MinValue < _minHisto ? statistic.MinValue : _minHisto;
                    _maxHisto = statistic.MaxValue > _maxHisto ? statistic.MaxValue : _maxHisto;

                    var sigmaLow = statistic.GetSigmaRangeLow(SigmaByIdx(comboboxSigmaHisto.SelectedIndex));
                    var sigmaHigh = statistic.GetSigmaRangeHigh(SigmaByIdx(comboboxSigmaHisto.SelectedIndex));

                    if (sigmaLow < _allsigmaLowHisto) _allsigmaLowHisto = sigmaLow;
                    if (sigmaHigh > _allsigmaHighHisto) _allsigmaHighHisto = sigmaHigh;

                    _itemTitleHisto += " & " + _selectedIds[i];
                }
            }

            //set the y axix
            if (radioSigmaHisto.IsChecked.Value) {
                ExecuteCmdSelectAxisSigmaHisto();
            } else if (radioMinMaxHisto.IsChecked.Value) {
                ExecuteCmdSelectAxisMinMaxHisto();
            } else if (radioLimitHisto.IsChecked.Value) {
                ExecuteCmdSelectAxisLimitHisto();
            } else {
                ExecuteCmdSelectAxisUserHisto();
            }
        }

        //default 100 bins, and enable outliers count, total 112bins
        (double[], double[], double) GetHistogramData(double start, double stop, IEnumerable<float> data) {
            var step = (stop - start) / 100;

            double[] range = new double[103];
            var actStart = start;// - step * 5;
            var actStop = stop;// + step * 5;

            for (int i = 0; i < 103; i++) {
                range[i] = start + (i - 1) * step;
            }
            double[] rangeCnt = new double[103];

            foreach (var f in data) {
                if (isInvalid(f)) continue;
                if (f < actStart) {
                    rangeCnt[0]++;
                } else if (f > actStop) {
                    rangeCnt[102]++;
                } else {
                    var idx = (int)Math.Round((f - actStart) / step) + 1;
                    rangeCnt[idx]++;
                }
            }

            return (rangeCnt, range, step);
        }

        List<float> GetZoomData(double start, double stop, IEnumerable<float> data)
        {
            List<float> zoomData = new List<float>();

            foreach (var f in data)
            {
                if (isInvalid(f))
                    continue;
                if (f > start && f < stop)
                {
                    zoomData.Add(f);
                }
            }

            return zoomData;
        }

        private void UpdateHistoSeries(float fstart, float fstop) {
            if (!_dataValid) return;
            if (isInvalid(fstart) || isInvalid(fstop)) return;
            if (_deviceCount == 0) return;

            histoChart.Plot.Clear();
            histoChart.Plot.Title(_itemTitleHisto, true, null, 12);
            histoChart.Plot.Legend(_ifShowLegendCheckBox, ScottPlot.Alignment.UpperRight);

            UpdateAxis_Histo();

            var da = StdDB.GetDataAcquire(_subData.StdFilePath);

            double maxCnt = 0;

            double start = fstart;
            double stop = fstop;
            if (start == stop) {
                start -= 1;
                stop += 1;
            }

            if (toggleSplitSite.IsEnabled && toggleSplitSite.IsChecked.Value) {
                var sites = da.GetSites();

                for (int i = 0; i < (sites.Length > 16 ? 16 : sites.Length); i++) {
                    var data = da.GetFilteredItemDataBySite(_selectedIds[0], _subData.FilterId, sites[i]);
                    if (data.Count() == 0 || !sitesChoosed.Contains(sites[i])) continue;
                    var histo = GetHistogramData(start, stop, data);

                    var color = SA.GetColor(i);

                    var bar = histoChart.Plot.AddBar(histo.Item1, histo.Item2, Color.FromArgb(color.A, color.R, color.G, color.B));
                    bar.BarWidth = histo.Item3 > 0 ? histo.Item3 : 1;
                    //bar.Label = $"S:{sites[i]}"; 20241122

                    if (i == 0) {
                        maxCnt = histo.Item1.Max();
                    } else {
                        if (maxCnt < histo.Item1.Max()) maxCnt = histo.Item1.Max();
                    }
                }

            } else {
                for (int i = 0; i < (_selectedIds.Count > 16 ? 16 : _selectedIds.Count); i++) {
                    var data = da.GetFilteredItemData(_selectedIds[i], _subData.FilterId);

                    var histo = GetHistogramData(start, stop, data);

                    var color = SA.GetColor(i);

                    var bar = histoChart.Plot.AddBar(histo.Item1.Skip(1).Take(101).ToArray(), histo.Item2.Skip(1).Take(101).ToArray(), Color.FromArgb(color.A, color.R, color.G, color.B));
                    bar.BarWidth = histo.Item3 > 0 ? histo.Item3 : 1;
                    bar.Label = _selectedIds[i];

                    var colorOutlier = SA.GetHistogramOutlierColor();
                    histoChart.Plot.AddBar(histo.Item1.Take(1).ToArray(), histo.Item2.Take(1).ToArray(), Color.FromArgb(colorOutlier.A, colorOutlier.R, colorOutlier.G, colorOutlier.B)).BarWidth = histo.Item3 > 0 ? histo.Item3 : 1;
                    histoChart.Plot.AddBar(histo.Item1.Skip(102).Take(1).ToArray(), histo.Item2.Skip(102).Take(1).ToArray(), Color.FromArgb(colorOutlier.A, colorOutlier.R, colorOutlier.G, colorOutlier.B)).BarWidth = histo.Item3 > 0 ? histo.Item3 : 1;


                    if (i == 0) {
                        maxCnt = histo.Item1.Max();
                    } else {
                        if (maxCnt < histo.Item1.Max()) maxCnt = histo.Item1.Max();
                    }
                }
            }

            var ov = 5 * (stop - start) / 100;
            if (ov == 0) ov = 1;
            var actStart = start - ov;
            var actStop = stop + ov;
            histoChart.Plot.SetAxisLimitsX(actStart, actStop);

            histoChart.Plot.AxisAutoY();
            histoChart.Refresh();
        }

        private void UpdateHistoSeriesForZoom(float fstart, float fstop)
        {
            if (!_dataValid)
                return;
            if (isInvalid(fstart) || isInvalid(fstop))
                return;
            if (_deviceCount == 0)
                return;

            histoChart.Plot.Clear();
            histoChart.Plot.Title(_itemTitleHisto, true, null, 12);
            histoChart.Plot.Legend(_ifShowLegendCheckBox, ScottPlot.Alignment.UpperRight);

            UpdateAxis_Histo();

            var da = StdDB.GetDataAcquire(_subData.StdFilePath);

            double maxCnt = 0;

            double start = fstart;
            double stop = fstop;
            if (start == stop)
            {
                start -= 1;
                stop += 1;
            }

            // deane
            List<float> allData = new List<float>();
            int TotalValidCnt = 0;
            if (toggleSplitSite.IsEnabled && toggleSplitSite.IsChecked.Value)
            {
                var sites = da.GetSites();

                for (int i = 0; i < (sites.Length > 16 ? 16 : sites.Length); i++)
                {
                    var data = da.GetFilteredItemDataBySite(
                        _selectedIds[0],
                        _subData.FilterId,
                        sites[i]
                    );
                    if (data.Count() == 0 || !sitesChoosed.Contains(sites[i]))
                        continue;
                    var histo = GetHistogramData(start, stop, data);

                    var color = SA.GetColor(i);

                    var bar = histoChart.Plot.AddBar(
                        histo.Item1.Skip(1).Take(101).ToArray(),
                        histo.Item2.Skip(1).Take(101).ToArray(),
                        Color.FromArgb(color.A, color.R, color.G, color.B)
                    );
                    bar.BarWidth = histo.Item3 > 0 ? histo.Item3 : 1;
                    //bar.Label = $"S:{sites[i]}";
                    // bar.IsVisible = false;
                    if (i == 0)
                    {
                        maxCnt = histo.Item1.Max();
                    }
                    else
                    {
                        if (maxCnt < histo.Item1.Max())
                            maxCnt = histo.Item1.Max();
                    }
                }
            }
            else
            {
                for (int i = 0; i < (_selectedIds.Count > 16 ? 16 : _selectedIds.Count); i++)
                {
                    var data = da.GetFilteredItemData(_selectedIds[i], _subData.FilterId);
                    // TotalValidCnt = data.Count<float>(); //
                    var histo = GetHistogramData(start, stop, data);

                    var color = SA.GetColor(i);

                    var bar = histoChart.Plot.AddBar(
                        histo.Item1.Skip(1).Take(101).ToArray(),
                        histo.Item2.Skip(1).Take(101).ToArray(),
                        Color.FromArgb(color.A, color.R, color.G, color.B)
                    );
                    bar.BarWidth = histo.Item3 > 0 ? histo.Item3 : 1;
                    bar.Label = _selectedIds[i];

                    //var colorOutlier = SA.GetHistogramOutlierColor();
                    //histoChart
                    //    .Plot.AddBar(
                    //        histo.Item1.Take(1).ToArray(),
                    //        histo.Item2.Take(1).ToArray(),
                    //        Color.FromArgb(
                    //            colorOutlier.A,
                    //            colorOutlier.R,
                    //            colorOutlier.G,
                    //            colorOutlier.B
                    //        )
                    //    )
                    //    .BarWidth = histo.Item3 > 0 ? histo.Item3 : 1;
                    //histoChart
                    //    .Plot.AddBar(
                    //        histo.Item1.Skip(102).Take(1).ToArray(),
                    //        histo.Item2.Skip(102).Take(1).ToArray(),
                    //        Color.FromArgb(
                    //            colorOutlier.A,
                    //            colorOutlier.R,
                    //            colorOutlier.G,
                    //            colorOutlier.B
                    //        )
                    //    )
                    //    .BarWidth = histo.Item3 > 0 ? histo.Item3 : 1;

                    if (i == 0)
                    {
                        maxCnt = histo.Item1.Max();
                    }
                    else
                    {
                        if (maxCnt < histo.Item1.Max())
                            maxCnt = histo.Item1.Max();
                    }
                }
            }
            //Deane
            allData = GetZoomData(
                start,
                stop,
                da.GetFilteredItemData(_selectedIds[0], _subData.FilterId)
            );
            ItemStatistic iStat = new ItemStatistic(allData, (float)start, (float)stop);
            var info = da.GetTestInfo(_selectedIds[0]);
            string failureRate = "0.0";
            if (da.ChipsCount > 0)
                failureRate = ((100.0 * iStat.PassCount) / da.ChipsCount).ToString("f3") + "%";
            _itemTitleHisto =
                $"{_selectedIds[0]}:{info.TestText}\nmean|{iStat.MeanValue:f3}  median|{iStat.MedianValue:f3}  σ|{iStat.Sigma:f3}  QTY|{iStat.PassCount}/{da.ChipsCount}={failureRate}";
            histoChart.Plot.Title(_itemTitleHisto, true, null, 12);

            var ov = 5 * (stop - start) / 100;
            if (ov == 0)
                ov = 1;
            var actStart = start - ov;
            var actStop = stop + ov;
            histoChart.Plot.SetAxisLimitsX(actStart, actStop);
            histoChart.Plot.AxisAutoY();
            histoChart.Refresh();
        }

        public void OnNavigatedTo(NavigationContext navigationContext) {
            var data = (SubData)navigationContext.Parameters["subData"];
            if (!_subData.Equals(data)) {
                _subData = data;

                _selectedIds = new List<string>((List<string>)navigationContext.Parameters["itemList"]);

                _ea.GetEvent<Event_FilterUpdated>().Subscribe(UpdateFilter);
                _ea.GetEvent<Event_ItemsSelected>().Subscribe(UpdateItems);

                UpdateData();

            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {

            return false;

            //var data = (SubData)navigationContext.Parameters["subData"];

            //return data.Equals(_subData);
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {
            _ea.GetEvent<Event_FilterUpdated>().Unsubscribe(UpdateFilter);
            _ea.GetEvent<Event_ItemsSelected>().Unsubscribe(UpdateItems);

        }
        ///<summary>
        /// Check if file is Good for writing
        ///</summary>
        ///<param name="filePath">File path</param>
        ///<returns></returns>
        public static bool IsFileGoodForWriting(string filePath) {
            FileStream stream = null;
            FileInfo file = new FileInfo(filePath);

            try {
                stream = file.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
            }
            catch (Exception) {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return false;
            } finally {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return true;
        }

        public SaveFileDialog CreateFileDialog(string filter) {
            var saveFileDialog = new SaveFileDialog {
                Filter = filter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            return saveFileDialog;
        }

        private bool GetAndCheckPath(string filter, string dftName, out string path) {
            var ret = false;
            var isGoodPath = false;
            var saveFileDialog = CreateFileDialog(filter);
            saveFileDialog.FileName = dftName;
            path = null;

            while (!isGoodPath) {
                if (saveFileDialog.ShowDialog() == true) {
                    if (IsFileGoodForWriting(saveFileDialog.FileName)) {
                        path = saveFileDialog.FileName;
                        isGoodPath = true;
                        ret = true;
                    } else {
                        System.Windows.MessageBox.Show(
                            "File is inaccesible for writing or you can not create file in this location, please choose another one.");
                    }
                } else {
                    isGoodPath = true;
                }
            }

            return ret;
        }

        void UpdateAxis_Trend() {
            if (isInvalid(_lowLimit) || !AxisLimitTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineLLimit_Trend);
            } else {
                lineLLimit_Trend = trendChart.Plot.AddHorizontalLine(_lowLimit, Color.Red, 2, ScottPlot.LineStyle.Solid);
            }
            if (isInvalid(_highLimit) || !AxisLimitTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineHLimit_Trend);
            } else {
                lineHLimit_Trend = trendChart.Plot.AddHorizontalLine(_highLimit, Color.Red, 2, ScottPlot.LineStyle.Solid);
            }
            if (isInvalid(_meanTrend) || !AxisMeanTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineMean_Trend);
            } else {
                lineMean_Trend = trendChart.Plot.AddHorizontalLine(_meanTrend, Color.Tomato, 2, ScottPlot.LineStyle.Solid);
            }
            if (isInvalid(_medianTrend) || !AxisMedianTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineMedian_Trend);
            } else {
                lineMedian_Trend = trendChart.Plot.AddHorizontalLine(_medianTrend, Color.DarkGoldenrod, 2, ScottPlot.LineStyle.Solid);
            }
            if (isInvalid(_minTrend) || !AxisMinMaxTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineMin_Trend);
            } else {
                lineMin_Trend = trendChart.Plot.AddHorizontalLine(_minTrend, Color.IndianRed, 2, ScottPlot.LineStyle.Solid);
            }

            if (isInvalid(_maxTrend) || !AxisMinMaxTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineMax_Trend);
            } else {
                lineMax_Trend = trendChart.Plot.AddHorizontalLine(_maxTrend, Color.IndianRed, 2, ScottPlot.LineStyle.Solid);
            }
            if (isInvalid(_sigma6LTrend) || !AxisSigma6Trend.IsChecked.Value) {
                trendChart.Plot.Remove(lineLSigma6_Trend);
            } else {
                lineLSigma6_Trend = trendChart.Plot.AddHorizontalLine(_sigma6LTrend, Color.DeepSkyBlue, 1, ScottPlot.LineStyle.Dash);
            }
            if (isInvalid(_sigma6HTrend) || !AxisSigma6Trend.IsChecked.Value) {
                trendChart.Plot.Remove(lineHSigma6_Trend);
            } else {
                lineHSigma6_Trend = trendChart.Plot.AddHorizontalLine(_sigma6HTrend, Color.DeepSkyBlue, 1, ScottPlot.LineStyle.Dash);
            }
            if (isInvalid(_sigma3LTrend) || !AxisSigma3Trend.IsChecked.Value) {
                trendChart.Plot.Remove(lineLSigma3_Trend);
            } else {
                lineLSigma3_Trend = trendChart.Plot.AddHorizontalLine(_sigma3LTrend, Color.Lime, 1, ScottPlot.LineStyle.Dash);
            }
            if (isInvalid(_sigma3HTrend) || !AxisSigma3Trend.IsChecked.Value) {
                trendChart.Plot.Remove(lineHSigma3_Trend);
            } else {
                lineHSigma3_Trend = trendChart.Plot.AddHorizontalLine(_sigma3HTrend, Color.Lime, 1, ScottPlot.LineStyle.Dash);
            }

        }

        void UpdateAxis_Histo() {
            if (isInvalid(_lowLimit) || !AxisLimitHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineLLimit_Histo);
            } else {
                lineLLimit_Histo = histoChart.Plot.AddVerticalLine(_lowLimit, Color.Red, 2, ScottPlot.LineStyle.Solid);
                lineLLimit_Histo.PositionLabel = true;
                lineLLimit_Histo.PositionLabelBackground = Color.MediumPurple;
                lineLLimit_Histo.PositionFormatter = _lowLimit => $"{_lowLimit:N3}";
            }
            if (isInvalid(_highLimit) || !AxisLimitHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineHLimit_Histo);
            } else {
                lineHLimit_Histo = histoChart.Plot.AddVerticalLine(_highLimit, Color.Red, 2, ScottPlot.LineStyle.Solid);
                lineHLimit_Histo.PositionLabel = true;
                lineHLimit_Histo.PositionLabelBackground = Color.MediumPurple;
                lineHLimit_Histo.PositionFormatter = _highLimit => $"{_highLimit:N3}";
            }
            if (isInvalid(_meanHisto) || !AxisMeanHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineMean_Histo);
            } else {
                lineMean_Histo = histoChart.Plot.AddVerticalLine(_meanHisto, Color.Tomato, 2, ScottPlot.LineStyle.Solid);
            }
            if (isInvalid(_medianHisto) || !AxisMedianHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineMedian_Histo);
            } else {
                lineMedian_Histo = histoChart.Plot.AddVerticalLine(_medianHisto, Color.DarkGoldenrod, 2, ScottPlot.LineStyle.Solid);
            }
            if (isInvalid(_minHisto) || !AxisMinMaxHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineMin_Histo);
            } else {
                lineMin_Histo = histoChart.Plot.AddVerticalLine(_minHisto, Color.IndianRed, 2, ScottPlot.LineStyle.Solid);
            }

            if (isInvalid(_maxHisto) || !AxisMinMaxHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineMax_Histo);
            } else {
                lineMax_Histo = histoChart.Plot.AddVerticalLine(_maxHisto, Color.IndianRed, 2, ScottPlot.LineStyle.Solid);
            }
            if (isInvalid(_sigma6LHisto) || !AxisSigma6Histo.IsChecked.Value) {
                histoChart.Plot.Remove(lineLSigma6_Histo);
            } else {
                lineLSigma6_Histo = histoChart.Plot.AddVerticalLine(_sigma6LHisto, Color.DeepSkyBlue, 1, ScottPlot.LineStyle.Dash);
            }
            if (isInvalid(_sigma6HHisto) || !AxisSigma6Histo.IsChecked.Value) {
                histoChart.Plot.Remove(lineHSigma6_Histo);
            } else {
                lineHSigma6_Histo = histoChart.Plot.AddVerticalLine(_sigma6HHisto, Color.DeepSkyBlue, 1, ScottPlot.LineStyle.Dash);
            }
            if (isInvalid(_sigma3LHisto) || !AxisSigma3Histo.IsChecked.Value) {
                histoChart.Plot.Remove(lineLSigma3_Histo);
            } else {
                lineLSigma3_Histo = histoChart.Plot.AddVerticalLine(_sigma3LHisto, Color.Lime, 1, ScottPlot.LineStyle.Dash);
            }
            if (isInvalid(_sigma3HHisto) || !AxisSigma3Histo.IsChecked.Value) {
                histoChart.Plot.Remove(lineHSigma3_Histo);
            } else {
                lineHSigma3_Histo = histoChart.Plot.AddVerticalLine(_sigma3HHisto, Color.Lime, 1, ScottPlot.LineStyle.Dash);
            }

        }

        void ExecuteCmdSelectAxisSigmaTrend() {
            if (!_dataValid) return;
            var ov = 0.05 * (_allsigmaHighTrend - _allsigmaLowTrend);
            if (ov == 0) ov = 1;
            trendChart.Plot.SetAxisLimitsY(_allsigmaLowTrend - ov, _allsigmaHighTrend + ov);

            textboxLRange.Text = _allsigmaLowTrend.ToString("f3");
            textboxHRange.Text = _allsigmaHighTrend.ToString("f3");

            trendChart.Plot.AxisAutoX();
            trendChart.Refresh();
        }

        void ExecuteCmdSelectAxisMinMaxTrend() {
            if (!_dataValid) return;
            var ov = 0.05 * (_maxTrend - _minTrend);
            if (ov == 0) ov = 1;
            trendChart.Plot.SetAxisLimitsY(_minTrend - ov, _maxTrend + ov);

            textboxLRange.Text = _minTrend.ToString("f3");
            textboxHRange.Text = _maxTrend.ToString("f3");

            trendChart.Plot.AxisAutoX();
            trendChart.Refresh();
        }

        void ExecuteCmdSelectAxisLimitTrend() {
            if (!_dataValid) return;
            float l = isInvalid(_lowLimit) ? _minTrend : _lowLimit;
            float h = isInvalid(_highLimit) ? _maxTrend : _highLimit;

            var ov = 0.1 * (h - l);
            if (ov == 0) ov = 1;
            trendChart.Plot.SetAxisLimitsY(l - ov, h + ov);

            textboxLRange.Text = l.ToString("f3");
            textboxHRange.Text = h.ToString("f3");

            trendChart.Plot.AxisAutoX();
            trendChart.Refresh();
        }

        void ExecuteCmdSelectAxisUserTrend() {
            if (!_dataValid) return;
            try {
                float.TryParse(textboxLRange.Text, out float l);
                float.TryParse(textboxHRange.Text, out float h);
                var ov = 0.1 * (h - l);
                if (ov == 0) ov = 1;
                trendChart.Plot.SetAxisLimitsY(l - ov, h + ov);
            }
            catch {
                System.Windows.MessageBox.Show("Wrong Limit");
            }
            trendChart.Plot.AxisAutoX();
            trendChart.Refresh();
        }

        void ExecuteCmdSelectAxisSigmaHisto() {
            if (!_dataValid) return;
            UpdateHistoSeries(_allsigmaLowHisto, _allsigmaHighHisto);

            textboxLRangeHisto.Text = _allsigmaLowHisto.ToString("f3");
            textboxHRangeHisto.Text = _allsigmaHighHisto.ToString("f3");
        }


        void ExecuteCmdSelectAxisMinMaxHisto() {
            if (!_dataValid) return;
            UpdateHistoSeries(_minHisto, _maxHisto);

            textboxLRangeHisto.Text = _minHisto.ToString("f3");
            textboxHRangeHisto.Text = _maxHisto.ToString("f3");
        }

        void ExecuteCmdSelectAxisLimitHisto() {
            if (!_dataValid) return;
            float l = isInvalid(_lowLimit) ? _minHisto : _lowLimit;
            float h = isInvalid(_highLimit) ? _maxHisto : _highLimit;

            UpdateHistoSeries(l, h);

            textboxLRangeHisto.Text = l.ToString();
            textboxHRangeHisto.Text = h.ToString();
        }

        void ExecuteCmdSelectAxisUserHisto() {
            if (!_dataValid) return;
            float l, h;
            try {
                float.TryParse(textboxLRangeHisto.Text, out l);
                float.TryParse(textboxHRangeHisto.Text, out h);
                UpdateHistoSeries(l, h);
            }
            catch {
                System.Windows.MessageBox.Show("Wrong Limit");
            }
        }

        void ExecuteCmdToggleSplitBySiteHisto() {
            if (!_dataValid) return;

            if (_selectedIds.Count == 1) {
                if (toggleSplitSite.IsEnabled && toggleSplitSite.IsChecked.Value)
                    _ifShowLegendCheckBox = true;
                else
                    _ifShowLegendCheckBox = false;
            } else {
                _ifShowLegendCheckBox = true;
            }

            UpdateHisto();
        }

        private void CmdSaveTrend_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (_selectedIds == null || _selectedIds.Count == 0) {
                System.Windows.MessageBox.Show("Select at list one item");
                return;
            }
            var txt = StdDB.GetDataAcquire(_subData.StdFilePath).GetTestInfo(_selectedIds[0]).TestText;

            string dftName = $"{_selectedIds[0]}_{txt}_Trend";
            if (_selectedIds.Count > 1) dftName += "_cmp";
            if (GetAndCheckPath("PNG | *.png", dftName, out string filePath)) {
                trendChart.Plot.SaveFig(filePath);
            }
        }

        private void CmdCopy_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (_selectedIds == null || _selectedIds.Count == 0) {
                System.Windows.MessageBox.Show("Select at list one item");
                return;
            }
            var image = trendChart.Plot.GetBitmap();
            System.Windows.Clipboard.SetImage(System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            image.GetHbitmap(),
            IntPtr.Zero,
            System.Windows.Int32Rect.Empty,
            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions()));

            _ea.GetEvent<Event_Log>().Publish("Copied to clipboard");
        }

        private void radioSigma_Checked(object sender, System.Windows.RoutedEventArgs e) {
            ExecuteCmdSelectAxisSigmaTrend();
        }

        private void comboboxSigma_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            UpdateTrend();
        }

        private void radioMinMax_Checked(object sender, System.Windows.RoutedEventArgs e) {
            ExecuteCmdSelectAxisMinMaxTrend();
        }

        private void radioLimit_Checked(object sender, System.Windows.RoutedEventArgs e) {
            ExecuteCmdSelectAxisLimitTrend();
        }

        private void radioUser_Checked(object sender, System.Windows.RoutedEventArgs e) {
            ExecuteCmdSelectAxisUserTrend();
        }

        private void buttonApplyTrendRange_Click(object sender, System.Windows.RoutedEventArgs e) {
            if(radioUser.IsChecked != true) {
                radioUser.IsChecked = true;
            } else {
                ExecuteCmdSelectAxisUserTrend();
            }
        }

        private void toggleOutlierTrend_Click(object sender, System.Windows.RoutedEventArgs e) {
            UpdateTrend();
        }

        private void comboboxOutlierSigma_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            UpdateTrend();
        }

        private void AxisLimitTrend_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_lowLimit) || !AxisLimitTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineLLimit_Trend);
            } else {
                lineLLimit_Trend = trendChart.Plot.AddHorizontalLine(_lowLimit, Color.Red, 2, ScottPlot.LineStyle.Solid);
            }
            if (isInvalid(_highLimit) || !AxisLimitTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineHLimit_Trend);
            } else {
                lineHLimit_Trend = trendChart.Plot.AddHorizontalLine(_highLimit, Color.Red, 2, ScottPlot.LineStyle.Solid);
            }
            trendChart.Refresh();
        }

        private void AxisMinMaxTrend_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_minTrend) || !AxisMinMaxTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineMin_Trend);
            } else {
                lineMin_Trend = trendChart.Plot.AddHorizontalLine(_minTrend, Color.IndianRed, 2, ScottPlot.LineStyle.Solid);
            }

            if (isInvalid(_maxTrend) || !AxisMinMaxTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineMax_Trend);
            } else {
                lineMax_Trend = trendChart.Plot.AddHorizontalLine(_maxTrend, Color.IndianRed, 2, ScottPlot.LineStyle.Solid);
            }

            trendChart.Refresh();
        }

        private void AxisMeanTrend_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_meanTrend) || !AxisMeanTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineMean_Trend);
            } else {
                lineMean_Trend = trendChart.Plot.AddHorizontalLine(_meanTrend, Color.Tomato, 2, ScottPlot.LineStyle.Solid);
            }

            trendChart.Refresh();
        }

        private void AxisMedianTrend_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_medianTrend) || !AxisMedianTrend.IsChecked.Value) {
                trendChart.Plot.Remove(lineMedian_Trend);
            } else {
                lineMedian_Trend = trendChart.Plot.AddHorizontalLine(_medianTrend, Color.DarkGoldenrod, 2, ScottPlot.LineStyle.Solid);
            }

            trendChart.Refresh();
        }

        private void AxisSigma6Trend_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_sigma6LTrend) || !AxisSigma6Trend.IsChecked.Value) {
                trendChart.Plot.Remove(lineLSigma6_Trend);
            } else {
                lineLSigma6_Trend = trendChart.Plot.AddHorizontalLine(_sigma6LTrend, Color.DeepSkyBlue, 1, ScottPlot.LineStyle.Dash);
            }
            if (isInvalid(_sigma6HTrend) || !AxisSigma6Trend.IsChecked.Value) {
                trendChart.Plot.Remove(lineHSigma6_Trend);
            } else {
                lineHSigma6_Trend = trendChart.Plot.AddHorizontalLine(_sigma6HTrend, Color.DeepSkyBlue, 1, ScottPlot.LineStyle.Dash);
            }

            trendChart.Refresh();
        }

        private void AxisSigma3Trend_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_sigma3LTrend) || !AxisSigma3Trend.IsChecked.Value) {
                trendChart.Plot.Remove(lineLSigma3_Trend);
            } else {
                lineLSigma3_Trend = trendChart.Plot.AddHorizontalLine(_sigma3LTrend, Color.Lime, 1, ScottPlot.LineStyle.Dash);
            }
            if (isInvalid(_sigma3HTrend) || !AxisSigma3Trend.IsChecked.Value) {
                trendChart.Plot.Remove(lineHSigma3_Trend);
            } else {
                lineHSigma3_Trend = trendChart.Plot.AddHorizontalLine(_sigma3HTrend, Color.Lime, 1, ScottPlot.LineStyle.Dash);
            }

            trendChart.Refresh();
        }

        private void buttonSaveHisto_Click(object sender, System.Windows.RoutedEventArgs e) {
            string filePath;
            if (_selectedIds == null || _selectedIds.Count == 0) {
                System.Windows.MessageBox.Show("Select at list one item");
                return;
            }
            var txt = StdDB.GetDataAcquire(_subData.StdFilePath).GetTestInfo(_selectedIds[0]).TestText;

            string dftName = $"{_selectedIds[0]}_{txt}_Histo";
            if (_selectedIds.Count > 1) dftName += "_cmp";
            if (GetAndCheckPath("PNG | *.png", dftName, out filePath)) {
                histoChart.Plot.SaveFig(filePath);
            }
        }

        private void buttonCopyHisto_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (_selectedIds == null || _selectedIds.Count == 0) {
                System.Windows.MessageBox.Show("Select at list one item");
                return;
            }
            var image = histoChart.Plot.GetBitmap();
            System.Windows.Clipboard.SetImage(System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            image.GetHbitmap(),
            IntPtr.Zero,
            System.Windows.Int32Rect.Empty,
            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions()));

            _ea.GetEvent<Event_Log>().Publish("Copied to clipboard");

        }

        private void radioSigmaHisto_Checked(object sender, System.Windows.RoutedEventArgs e) {
            ExecuteCmdSelectAxisSigmaHisto();
        }

        private void comboboxSigmaHisto_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            UpdateHisto();
        }

        private void radioMinMaxHisto_Checked(object sender, System.Windows.RoutedEventArgs e) {
            ExecuteCmdSelectAxisMinMaxHisto();
        }

        private void radioLimitHisto_Checked(object sender, System.Windows.RoutedEventArgs e) {
            ExecuteCmdSelectAxisLimitHisto();
        }

        private void radioUserHisto_Checked(object sender, System.Windows.RoutedEventArgs e) {
            ExecuteCmdSelectAxisUserHisto();
        }

        private void buttonApplyRangeHisto_Click(object sender, System.Windows.RoutedEventArgs e) {
            if(radioUserHisto.IsChecked != true) {
                radioUserHisto.IsChecked = true;
            } else {
                ExecuteCmdSelectAxisUserHisto();
            }
        }

        private void toggleOutlierHisto_Click(object sender, System.Windows.RoutedEventArgs e) {
            UpdateHisto();
        }

        private void comboboxOutlierSigmaHisto_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            UpdateHisto();
        }

        private void toggleSplitSite_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (toggleSplitSite.IsChecked ?? false) //20241122
                allsites.SelectAll();
            ExecuteCmdToggleSplitBySiteHisto();
        }

        private void AxisLimitHisto_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_lowLimit) || !AxisLimitHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineLLimit_Histo);
            } else {
                lineLLimit_Histo = histoChart.Plot.AddVerticalLine(_lowLimit, Color.Red, 2, ScottPlot.LineStyle.Solid);
            }
            if (isInvalid(_highLimit) || !AxisLimitHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineHLimit_Histo);
            } else {
                lineHLimit_Histo = histoChart.Plot.AddVerticalLine(_highLimit, Color.Red, 2, ScottPlot.LineStyle.Solid);
            }
          
            histoChart.Refresh();
        }

        private void AxisMinMaxHisto_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_minHisto) || !AxisMinMaxHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineMin_Histo);
            } else {
                lineMin_Histo = histoChart.Plot.AddVerticalLine(_minHisto, Color.IndianRed, 2, ScottPlot.LineStyle.Solid);
            }

            if (isInvalid(_maxHisto) || !AxisMinMaxHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineMax_Histo);
            } else {
                lineMax_Histo = histoChart.Plot.AddVerticalLine(_maxHisto, Color.IndianRed, 2, ScottPlot.LineStyle.Solid);
            }

            histoChart.Refresh();
        }

        private void AxisMeanHisto_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_meanHisto) || !AxisMeanHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineMean_Histo);
            } else {
                lineMean_Histo = histoChart.Plot.AddVerticalLine(_meanHisto, Color.Tomato, 2, ScottPlot.LineStyle.Solid);
            }

            histoChart.Refresh();
        }

        private void AxisMedianHisto_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_medianHisto) || !AxisMedianHisto.IsChecked.Value) {
                histoChart.Plot.Remove(lineMedian_Histo);
            } else {
                lineMedian_Histo = histoChart.Plot.AddVerticalLine(_medianHisto, Color.DarkGoldenrod, 2, ScottPlot.LineStyle.Solid);
            }

            histoChart.Refresh();
        }

        private void AxisSigma6Histo_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_sigma6LHisto) || !AxisSigma6Histo.IsChecked.Value) {
                histoChart.Plot.Remove(lineLSigma6_Histo);
            } else {
                lineLSigma6_Histo = histoChart.Plot.AddVerticalLine(_sigma6LHisto, Color.DeepSkyBlue, 1, ScottPlot.LineStyle.Dash);
            }
            if (isInvalid(_sigma6HHisto) || !AxisSigma6Histo.IsChecked.Value) {
                histoChart.Plot.Remove(lineHSigma6_Histo);
            } else {
                lineHSigma6_Histo = histoChart.Plot.AddVerticalLine(_sigma6HHisto, Color.DeepSkyBlue, 1, ScottPlot.LineStyle.Dash);
            }

            histoChart.Refresh();
        }

        // 20241122
        private void allsites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sitesChoosed.Clear();
            foreach (int a in allsites.SelectedItems)
            {
                sitesChoosed.Add(a);
            }

            ExecuteCmdToggleSplitBySiteHisto();
        }

        private void AxisSigma3Histo_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (isInvalid(_sigma3LHisto) || !AxisSigma3Histo.IsChecked.Value) {
                histoChart.Plot.Remove(lineLSigma3_Histo);
            } else {
                lineLSigma3_Histo = histoChart.Plot.AddVerticalLine(_sigma3LHisto, Color.Lime, 1, ScottPlot.LineStyle.Dash);
            }
            if (isInvalid(_sigma3HHisto) || !AxisSigma3Histo.IsChecked.Value) {
                histoChart.Plot.Remove(lineHSigma3_Histo);
            } else {
                lineHSigma3_Histo = histoChart.Plot.AddVerticalLine(_sigma3HHisto, Color.Lime, 1, ScottPlot.LineStyle.Dash);
            }

            histoChart.Refresh();
        }

        private void trendChart_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            trendChart.Plot.AxisAutoX();
            trendChart.Refresh();
        }

        private void histoChart_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            histoChart.Plot.AxisAutoY();
            histoChart.Refresh();
        }

    }

}
