Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.DashboardCommon

Namespace Dashboard_CreateRangeFilter
	Partial Public Class Form1
		Private Function CreatePivot(ByVal dataSource As DataSource) As PivotDashboardItem

			' Creates a pivot dashboard item and specifies its data source.
			Dim pivot As New PivotDashboardItem()
			pivot.DataSource = dataSource

			' Specifies dimensions that provide pivot column and row headers.
			pivot.Columns.AddRange(New Dimension("Country"), New Dimension("Sales Person"))
			pivot.Rows.AddRange(New Dimension("CategoryName"), New Dimension("ProductName"))

			' Specifies measures whose data is used to calculate pivot cell values.
			pivot.Values.AddRange(New Measure("Extended Price"), New Measure("Quantity"))

			' Specifies the default expanded state of pivot column field values.
			pivot.AutoExpandColumnGroups = True

			Return pivot
		End Function
	End Class
End Namespace
