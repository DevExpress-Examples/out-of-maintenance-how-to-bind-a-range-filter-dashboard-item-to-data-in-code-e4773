Imports DevExpress.DashboardCommon
Imports DevExpress.DataAccess.Excel
Imports DevExpress.XtraEditors
Imports System

Namespace Dashboard_CreateRangeFilter
	Partial Public Class Form1
		Inherits XtraForm

		Public Sub New()
			InitializeComponent()
		End Sub
		Private Function CreateRangeFilter(ByVal dataSource As IDashboardDataSource) As RangeFilterDashboardItem
			' Create a Range Filter dashboard item and specify its data source.
			Dim rangeFilter As New RangeFilterDashboardItem()
			rangeFilter.DataSource = dataSource
			' Create a new series of the Area type and add this series to the Series collection to
			' display it within the Range Filter.
			Dim salesAmountSeries As New SimpleSeries(SimpleSeriesType.Area)
			rangeFilter.Series.Add(salesAmountSeries)
			' Specify a measure to provide data used to calculate the Y-coordinate of the data points.
			salesAmountSeries.Value = New Measure("Extended Price")
			' Specify a dimension to provide Range Filter argument values.
			rangeFilter.Argument = New Dimension("OrderDate")
			' Specify a group interval for argument values.
			rangeFilter.Argument.DateTimeGroupInterval = DateTimeGroupInterval.MonthYear
			' Restrict the displayed data.
			rangeFilter.FilterString = "[OrderDate] > #2018-01-01#"
			' Add predefined ranges to the context menu.
			' You can show the item caption and use the Select Date Time Periods drop-down button to apply predefined ranges.
			rangeFilter.DateTimePeriods.AddRange(DateTimePeriod.CreateLastYear(), DateTimePeriod.CreateNextMonths("Next 3 Months", 3), New DateTimePeriod With {.Name = "Year To Date", .Start = New FlowDateTimePeriodLimit(DateTimeInterval.Year, 0), .End = New FlowDateTimePeriodLimit(DateTimeInterval.Day, 1)}, New DateTimePeriod With {.Name = "Jul-18-2018 - Jan-18-2019", .Start = New FixedDateTimePeriodLimit(New Date(2018, 7, 18)), .End = New FixedDateTimePeriodLimit(New Date(2019, 1, 18))})
			' Specify the period selected when the control is initialized.
			rangeFilter.DefaultDateTimePeriodName = "Year To Date"
			' The caption is initially hidden. Uncomment the line to show the caption.
			'rangeFilter.ShowCaption = true;
			Return rangeFilter
		End Function

		Private Function CreatePivot(ByVal dataSource As IDashboardDataSource) As PivotDashboardItem

			' Create a pivot dashboard item and specify its data source.
			Dim pivot As New PivotDashboardItem()
			pivot.DataSource = dataSource

			' Specify dimensions that provide pivot column and row headers.
			pivot.Columns.AddRange(New Dimension("Country"), New Dimension("Sales Person"))
			pivot.Rows.AddRange(New Dimension("CategoryName"), New Dimension("ProductName"))

			' Specify measures whose data is used to calculate pivot cell values.
			pivot.Values.AddRange(New Measure("Extended Price"), New Measure("Quantity"))

			' Specify the default expanded state of pivot column field values.
			pivot.AutoExpandColumnGroups = True

			Return pivot
		End Function

		Private Function CreateExcelDataSource() As DashboardExcelDataSource
			Dim excelDataSource As New DashboardExcelDataSource()
			excelDataSource.FileName = "SalesPerson.xlsx"
			Dim worksheetSettings As New ExcelWorksheetSettings("Data")
			excelDataSource.SourceOptions = New ExcelSourceOptions(worksheetSettings)
			excelDataSource.Fill()
			Return excelDataSource
		End Function
		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

			' Create a dashboard and display it in the dashboard viewer.
			dashboardViewer1.Dashboard = New Dashboard()

			' Create a data source and add it to the dashboard data source collection.
			Dim dataSource As DashboardExcelDataSource = CreateExcelDataSource()
			dashboardViewer1.Dashboard.DataSources.Add(dataSource)

			' Create a Range Filter dashboard item with the specified data source 
			' and add it to the Items collection to display within the dashboard.
			Dim rangeFilter As RangeFilterDashboardItem = CreateRangeFilter(dataSource)
			dashboardViewer1.Dashboard.Items.Add(rangeFilter)

			' Create a pivot and add it to the dashboard. 
			Dim pivot As PivotDashboardItem = CreatePivot(dataSource)
			dashboardViewer1.Dashboard.Items.Add(pivot)

			' Create the dashboard layout.
			dashboardViewer1.Dashboard.RebuildLayout()
			dashboardViewer1.Dashboard.LayoutRoot.FindRecursive(rangeFilter).Weight = 20
			dashboardViewer1.Dashboard.LayoutRoot.FindRecursive(pivot).Weight = 80
			dashboardViewer1.Dashboard.LayoutRoot.Orientation = DashboardLayoutGroupOrientation.Vertical

			dashboardViewer1.ReloadData()
		End Sub
	End Class
End Namespace