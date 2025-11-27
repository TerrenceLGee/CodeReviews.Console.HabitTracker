using System.ComponentModel.DataAnnotations;

namespace HabitTracker.TerrenceLGee.HabitModels;

public enum UnitOfMeasurement
{
    [Display(Name = "Minutes")]
    Minutes,
    [Display(Name = "Hours")]
    Hours,
    [Display(Name = "Kilometers")]
    Kilometers,
    [Display(Name = "Meters")]
    Meters,
    [Display(Name = "Centimeters")]
    Centimeters,
    [Display(Name = "Millimeters")]
    Millimeters,
    [Display(Name = "Feet")]
    Feet,
    [Display(Name = "Inches")]
    Inches,
    [Display(Name = "Yards")]
    Yards,
    [Display(Name = "Miles")]
    Miles
}