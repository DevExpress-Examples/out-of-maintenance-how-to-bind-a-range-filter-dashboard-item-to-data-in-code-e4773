using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.DashboardCommon;

namespace Dashboard_CreateRangeFilter {
    public partial class Form1 {
        private PivotDashboardItem CreatePivot(DashboardObjectDataSource dataSource) {

            // Creates a pivot dashboard item and specifies its data source.
            PivotDashboardItem pivot = new PivotDashboardItem();
            pivot.DataSource = dataSource;

            // Specifies dimensions that provide pivot column and row headers.
            pivot.Columns.AddRange(new Dimension("Country"), new Dimension("Sales Person"));
            pivot.Rows.AddRange(new Dimension("CategoryName"), new Dimension("ProductName"));

            // Specifies measures whose data is used to calculate pivot cell values.
            pivot.Values.AddRange(new Measure("Extended Price"), new Measure("Quantity"));

            // Specifies the default expanded state of pivot column field values.
            pivot.AutoExpandColumnGroups = true;

            return pivot;
        }
    }
}
