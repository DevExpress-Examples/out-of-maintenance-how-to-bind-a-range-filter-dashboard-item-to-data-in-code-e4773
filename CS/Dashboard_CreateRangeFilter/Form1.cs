using System;
using System.Windows.Forms;
using DevExpress.DashboardCommon;
using DevExpress.DataAccess;

namespace Dashboard_CreateRangeFilter {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        private RangeFilterDashboardItem CreateRangeFilter(DataSource dataSource) {

            // Creates a Range Filter dashboard item and specifies its data source.
            RangeFilterDashboardItem rangeFilter = new RangeFilterDashboardItem();            
            rangeFilter.DataSource = dataSource;

            // Creates a new series of the Area type and adds this series to the Series collection to
            // display it within the Range Filter.
            SimpleSeries salesAmountSeries = new SimpleSeries(SimpleSeriesType.Area);
            rangeFilter.Series.Add(salesAmountSeries);

            // Specifies a measure to provide data used to calculate the Y-coordinate of the data points.
            salesAmountSeries.Value = new Measure("Extended Price");

            // Specifies a dimension to provide Range Filter argument values.
            rangeFilter.Argument = new Dimension("OrderDate");

            // Specifies a group interval for argument values.
            rangeFilter.Argument.DateTimeGroupInterval = DateTimeGroupInterval.MonthYear;           

            return rangeFilter;
        }
        private void Form1_Load(object sender, EventArgs e) {

            // Creates a dashboard and sets it as the currently opened dashboard in the dashboard viewer.
            dashboardViewer1.Dashboard = new Dashboard();

            // Creates a data source and adds it to the dashboard data source collection.
            DataSource dataSource = new DataSource("Sales Person");
            dashboardViewer1.Dashboard.DataSources.Add(dataSource);

            // Creates a Range Filter dashboard item with the specified data source 
            // and adds it to the Items collection to display within the dashboard.
            RangeFilterDashboardItem rangeFilter = CreateRangeFilter(dataSource);
            dashboardViewer1.Dashboard.Items.Add(rangeFilter);

            // Creates a pivot and adds it to the dashboard. 
            // Range Filter applies filtering to pivot data.
            PivotDashboardItem pivot = CreatePivot(dataSource);
            dashboardViewer1.Dashboard.Items.Add(pivot);

            // Reloads data in the data sources.
            dashboardViewer1.ReloadData();
        }
        private void dashboardViewer1_DataLoading(object sender, DataLoadingEventArgs e) {

            // Specifies data for the current data source.
            e.Data = (new nwindDataSetTableAdapters.SalesPersonTableAdapter()).GetData();
        }
    }
}