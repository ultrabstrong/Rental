using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rental.WebApp.Extensions;

internal static class ControllerExtensions
{
    public static void AddUSStatesToViewBag(this Controller controller)
    {
        controller.ViewBag.USStates = new List<SelectListItem>
        {
            new() { Text = "-", Value = "" },
            new() { Text = "Alabama (AL)", Value = "AL" },
            new() { Text = "Alaska (AK)", Value = "AK" },
            new() { Text = "Arizona (AZ)", Value = "AZ" },
            new() { Text = "Arkansas (AR)", Value = "AR" },
            new() { Text = "California (CA)", Value = "CA" },
            new() { Text = "Colorado (CO)", Value = "CO" },
            new() { Text = "Connecticut (CT)", Value = "CT" },
            new() { Text = "Delaware (DE)", Value = "DE" },
            new() { Text = "Florida (FL)", Value = "FL" },
            new() { Text = "Georgia (GA)", Value = "GA" },
            new() { Text = "Hawaii (HI)", Value = "HI" },
            new() { Text = "Idaho (ID)", Value = "ID" },
            new() { Text = "Illinois (IL)", Value = "IL" },
            new() { Text = "Indiana (IN)", Value = "IN" },
            new() { Text = "Iowa (IA)", Value = "IA" },
            new() { Text = "Kansas (KS)", Value = "KS" },
            new() { Text = "Kentucky (KY)", Value = "KY" },
            new() { Text = "Louisiana (LA)", Value = "LA" },
            new() { Text = "Maine (ME)", Value = "ME" },
            new() { Text = "Maryland (MD)", Value = "MD" },
            new() { Text = "Massachusetts (MA)", Value = "MA" },
            new() { Text = "Michigan (MI)", Value = "MI" },
            new() { Text = "Minnesota (MN)", Value = "MN" },
            new() { Text = "Mississippi (MS)", Value = "MS" },
            new() { Text = "Missouri (MO)", Value = "MO" },
            new() { Text = "Montana (MT)", Value = "MT" },
            new() { Text = "Nebraska (NE)", Value = "NE" },
            new() { Text = "Nevada (NV)", Value = "NV" },
            new() { Text = "New Hampshire (NH)", Value = "NH" },
            new() { Text = "New Jersey (NJ)", Value = "NJ" },
            new() { Text = "New Mexico (NM)", Value = "NM" },
            new() { Text = "New York (NY)", Value = "NY" },
            new() { Text = "North Carolina (NC)", Value = "NC" },
            new() { Text = "North Dakota (ND)", Value = "ND" },
            new() { Text = "Ohio (OH)", Value = "OH" },
            new() { Text = "Oklahoma (OK)", Value = "OK" },
            new() { Text = "Oregon (OR)", Value = "OR" },
            new() { Text = "Pennsylvania (PA)", Value = "PA" },
            new() { Text = "Rhode Island (RI)", Value = "RI" },
            new() { Text = "South Carolina (SC)", Value = "SC" },
            new() { Text = "South Dakota (SD)", Value = "SD" },
            new() { Text = "Tennessee (TN)", Value = "TN" },
            new() { Text = "Texas (TX)", Value = "TX" },
            new() { Text = "Utah (UT)", Value = "UT" },
            new() { Text = "Vermont (VT)", Value = "VT" },
            new() { Text = "Virginia (VA)", Value = "VA" },
            new() { Text = "Washington (WA)", Value = "WA" },
            new() { Text = "West Virginia (WV)", Value = "WV" },
            new() { Text = "Wisconsin (WI)", Value = "WI" },
            new() { Text = "Wyoming (WY)", Value = "WY" },
            new() { Text = "District of Columbia (DC)", Value = "DC" }
        };
    }
}