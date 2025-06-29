using System.ComponentModel;

namespace UGCL.Data.Models;

[Description("A sample model provided by the Coalesce template. Remove this when you start building your real data model.")]
public class Widget
    : TrackingBase
{
    public int WidgetId { get; set; }

    public required string Name { get; set; }

    public required WidgetCategory Category { get; set; }

    public DateTimeOffset? InventedOn { get; set; }
}

public enum WidgetCategory
{
    Whizbangs,
    Sprecklesprockets,
    Discombobulators,
}
