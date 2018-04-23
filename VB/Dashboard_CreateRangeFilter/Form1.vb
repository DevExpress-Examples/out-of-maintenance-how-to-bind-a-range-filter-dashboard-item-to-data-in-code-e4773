Imports System
Imports System.Windows.Forms
Imports DevExpress.DashboardCommon
Imports DevExpress.DataAccess

Namespace Dashboard_CreateRangeFilter
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub
        Private Function CreateRangeFilter(ByVal dataSource As DashboardObjectDataSource) _
            As RangeFilterDashboardItem

            ' Creates a Range Filter dashboard item and specifies its data source.
            Dim rangeFilter As New RangeFilterDashboardItem()
            rangeFilter.DataSource = dataSource

            ' Creates a new series of the Area type and adds this series to the Series collection to
            ' display it within the Range Filter.
            Dim salesAmountSeries As New SimpleSeries(SimpleSeriesType.Area)
            rangeFilter.Series.Add(salesAmountSeries)

            ' Specifies a measure to provide data used to calculate the Y-coordinate of the data points.
            salesAmountSeries.Value = New Measure("Extended Price")

            ' Specifies a dimension to provide Range Filter argument values.
            rangeFilter.Argument = New Dimension("OrderDate")

            ' Specifies a group interval for argument values.
            rangeFilter.Argument.DateTimeGroupInterval = DateTimeGroupInterval.MonthYear

            Return rangeFilter
        End Function
        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Creates a dashboard and sets it as the currently opened dashboard in the dashboard viewer.
            dashboardViewer1.Dashboard = New Dashboard()

            ' Creates a data source and adds it to the dashboard data source collection.
            Dim dataSource As New DashboardObjectDataSource()
            dataSource.DataSource = (New nwindDataSetTableAdapters.SalesPersonTableAdapter()).GetData()
            dashboardViewer1.Dashboard.DataSources.Add(dataSource)

            ' Creates a Range Filter dashboard item with the specified data source 
            ' and adds it to the Items collection to display within the dashboard.
            Dim rangeFilter As RangeFilterDashboardItem = CreateRangeFilter(dataSource)
            dashboardViewer1.Dashboard.Items.Add(rangeFilter)

            ' Creates a pivot and adds it to the dashboard. 
            ' Range Filter applies filtering to pivot data.
            Dim pivot As PivotDashboardItem = CreatePivot(dataSource)
            dashboardViewer1.Dashboard.Items.Add(pivot)

            ' Reloads data in the data sources.
            dashboardViewer1.ReloadData()
        End Sub
    End Class
End Namespace