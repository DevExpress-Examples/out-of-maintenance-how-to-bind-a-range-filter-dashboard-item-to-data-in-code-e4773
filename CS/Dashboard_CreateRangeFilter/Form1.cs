using DevExpress.DashboardCommon;
using DevExpress.DataAccess.Excel;
using DevExpress.XtraEditors;
using System;

namespace Dashboard_CreateRangeFilter
{
    public partial class Form1 : XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        private RangeFilterDashboardItem CreateRangeFilter(IDashboardDataSource dataSource)
        {
            // Create a Range Filter dashboard item and specify its data source.
            RangeFilterDashboardItem rangeFilter = new RangeFilterDashboardItem();
            rangeFilter.DataSource = dataSource;
            // Create a new series of the Area type and add this series to the Series collection to
            // display it within the Range Filter.
            SimpleSeries salesAmountSeries = new SimpleSeries(SimpleSeriesType.Area);
            rangeFilter.Series.Add(salesAmountSeries);
            // Specify a measure to provide data used to calculate the Y-coordinate of the data points.
            salesAmountSeries.Value = new Measure("Extended Price");
            // Specify a dimension to provide Range Filter argument values.
            rangeFilter.Argument = new Dimension("OrderDate");
            // Specify a group interval for argument values.
            rangeFilter.Argument.DateTimeGroupInterval = DateTimeGroupInterval.MonthYear;
            // Restrict the displayed data.
            rangeFilter.FilterString = "[OrderDate] > #2018-01-01#";
            // Add predefined ranges to the context menu.
            // You can show the item caption and use the Select Date Time Periods drop-down button to apply predefined ranges.
            rangeFilter.DateTimePeriods.AddRange(
                DateTimePeriod.CreateLastYear(),
                DateTimePeriod.CreateNextMonths("Next 3 Months", 3),
                new DateTimePeriod
                { Name = "Year To Date",
                  Start = new FlowDateTimePeriodLimit(DateTimeInterval.Year, 0),
                  End = new FlowDateTimePeriodLimit(DateTimeInterval.Day, 1)
                },
                new DateTimePeriod
                { Name = "Jul-18-2018 - Jan-18-2019",
                  Start = new FixedDateTimePeriodLimit(new DateTime(2018, 7, 18)),
                  End = new FixedDateTimePeriodLimit(new DateTime(2019, 1, 18)) }
                );
            // Specify the period selected when the control is initialized.
            rangeFilter.DefaultDateTimePeriodName = "Year To Date";
            // The caption is initially hidden. Uncomment the line to show the caption.
            //rangeFilter.ShowCaption = true;
            return rangeFilter;
        }

        private PivotDashboardItem CreatePivot(IDashboardDataSource dataSource)
        {

            // Create a pivot dashboard item and specify its data source.
            PivotDashboardItem pivot = new PivotDashboardItem();
            pivot.DataSource = dataSource;

            // Specify dimensions that provide pivot column and row headers.
            pivot.Columns.AddRange(new Dimension("Country"), new Dimension("Sales Person"));
            pivot.Rows.AddRange(new Dimension("CategoryName"), new Dimension("ProductName"));

            // Specify measures whose data is used to calculate pivot cell values.
            pivot.Values.AddRange(new Measure("Extended Price"), new Measure("Quantity"));

            // Specify the default expanded state of pivot column field values.
            pivot.AutoExpandColumnGroups = true;

            return pivot;
        }

        private DashboardExcelDataSource CreateExcelDataSource()
        {
            DashboardExcelDataSource excelDataSource = new DashboardExcelDataSource();
            excelDataSource.FileName = "SalesPerson.xlsx";
            ExcelWorksheetSettings worksheetSettings = new ExcelWorksheetSettings("Data");
            excelDataSource.SourceOptions = new ExcelSourceOptions(worksheetSettings);
            excelDataSource.Fill();
            return excelDataSource;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            // Create a dashboard and display it in the dashboard viewer.
            dashboardViewer1.Dashboard = new Dashboard();

            // Create a data source and add it to the dashboard data source collection.
            DashboardExcelDataSource dataSource = CreateExcelDataSource();
            dashboardViewer1.Dashboard.DataSources.Add(dataSource);

            // Create a Range Filter dashboard item with the specified data source 
            // and add it to the Items collection to display within the dashboard.
            RangeFilterDashboardItem rangeFilter = CreateRangeFilter(dataSource);
            dashboardViewer1.Dashboard.Items.Add(rangeFilter);

            // Create a pivot and add it to the dashboard. 
            PivotDashboardItem pivot = CreatePivot(dataSource);
            dashboardViewer1.Dashboard.Items.Add(pivot);

            // Create the dashboard layout.
            dashboardViewer1.Dashboard.RebuildLayout();
            dashboardViewer1.Dashboard.LayoutRoot.FindRecursive(rangeFilter).Weight = 20;
            dashboardViewer1.Dashboard.LayoutRoot.FindRecursive(pivot).Weight = 80;
            dashboardViewer1.Dashboard.LayoutRoot.Orientation = DashboardLayoutGroupOrientation.Vertical;

            dashboardViewer1.ReloadData();
        }
    }
}