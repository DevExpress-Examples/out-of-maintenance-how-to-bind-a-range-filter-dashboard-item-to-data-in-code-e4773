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
            RangeFilterDashboardItem rangeFilter = new RangeFilterDashboardItem();
            rangeFilter.DataSource = dataSource;
            SimpleSeries salesAmountSeries = new SimpleSeries(SimpleSeriesType.Area);
            rangeFilter.Series.Add(salesAmountSeries);
            salesAmountSeries.Value = new Measure("Extended Price");
            rangeFilter.Argument = new Dimension("OrderDate");
            rangeFilter.Argument.DateTimeGroupInterval = DateTimeGroupInterval.MonthYear;
            rangeFilter.FilterString = "[OrderDate] > #2018-01-01#";
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
            rangeFilter.DefaultDateTimePeriodName = "Year To Date";
          // The caption is initially hidden. Uncomment the line below to show the caption.
          //rangeFilter.ShowCaption = true;
            return rangeFilter;
        }

        private PivotDashboardItem CreatePivot(IDashboardDataSource dataSource)
        {
            PivotDashboardItem pivot = new PivotDashboardItem();
            pivot.DataSource = dataSource;
            pivot.Columns.AddRange(new Dimension("Country"), new Dimension("Sales Person"));
            pivot.Rows.AddRange(new Dimension("CategoryName"), new Dimension("ProductName"));
            pivot.Values.AddRange(new Measure("Extended Price"), new Measure("Quantity"));
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
            Dashboard dashboard = new Dashboard();

            DashboardExcelDataSource dataSource = CreateExcelDataSource();
            dashboard.DataSources.Add(dataSource);
            RangeFilterDashboardItem rangeFilter = CreateRangeFilter(dataSource);
            dashboard.Items.Add(rangeFilter);
            PivotDashboardItem pivot = CreatePivot(dataSource);
            dashboard.Items.Add(pivot);

            // Create the dashboard layout.
            dashboard.RebuildLayout();
            dashboard.LayoutRoot.FindRecursive(rangeFilter).Weight = 20;
            dashboard.LayoutRoot.FindRecursive(pivot).Weight = 80;
            dashboard.LayoutRoot.Orientation = DashboardLayoutGroupOrientation.Vertical;

            dashboardViewer1.Dashboard = dashboard;
            dashboardViewer1.ReloadData();
        }
    }
}