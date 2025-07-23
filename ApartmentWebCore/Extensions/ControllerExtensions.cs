using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApartmentWeb.Extensions;

public static class ControllerExtensions
{
    public static void AddUSStatesToViewBag(this Controller controller)
    {
        controller.ViewBag.USStates = new List<SelectListItem>
        {
            new SelectListItem { Text = "-", Value = "" },
            new SelectListItem { Text = "Alabama (AL)", Value = "AL" },
            new SelectListItem { Text = "Alaska (AK)", Value = "AK" },
            new SelectListItem { Text = "Arizona (AZ)", Value = "AZ" },
            new SelectListItem { Text = "Arkansas (AR)", Value = "AR" },
            new SelectListItem { Text = "California (CA)", Value = "CA" },
            new SelectListItem { Text = "Colorado (CO)", Value = "CO" },
            new SelectListItem { Text = "Connecticut (CT)", Value = "CT" },
            new SelectListItem { Text = "Delaware (DE)", Value = "DE" },
            new SelectListItem { Text = "Florida (FL)", Value = "FL" },
            new SelectListItem { Text = "Georgia (GA)", Value = "GA" },
            new SelectListItem { Text = "Hawaii (HI)", Value = "HI" },
            new SelectListItem { Text = "Idaho (ID)", Value = "ID" },
            new SelectListItem { Text = "Illinois (IL)", Value = "IL" },
            new SelectListItem { Text = "Indiana (IN)", Value = "IN" },
            new SelectListItem { Text = "Iowa (IA)", Value = "IA" },
            new SelectListItem { Text = "Kansas (KS)", Value = "KS" },
            new SelectListItem { Text = "Kentucky (KY)", Value = "KY" },
            new SelectListItem { Text = "Louisiana (LA)", Value = "LA" },
            new SelectListItem { Text = "Maine (ME)", Value = "ME" },
            new SelectListItem { Text = "Maryland (MD)", Value = "MD" },
            new SelectListItem { Text = "Massachusetts (MA)", Value = "MA" },
            new SelectListItem { Text = "Michigan (MI)", Value = "MI" },
            new SelectListItem { Text = "Minnesota (MN)", Value = "MN" },
            new SelectListItem { Text = "Mississippi (MS)", Value = "MS" },
            new SelectListItem { Text = "Missouri (MO)", Value = "MO" },
            new SelectListItem { Text = "Montana (MT)", Value = "MT" },
            new SelectListItem { Text = "Nebraska (NE)", Value = "NE" },
            new SelectListItem { Text = "Nevada (NV)", Value = "NV" },
            new SelectListItem { Text = "New Hampshire (NH)", Value = "NH" },
            new SelectListItem { Text = "New Jersey (NJ)", Value = "NJ" },
            new SelectListItem { Text = "New Mexico (NM)", Value = "NM" },
            new SelectListItem { Text = "New York (NY)", Value = "NY" },
            new SelectListItem { Text = "North Carolina (NC)", Value = "NC" },
            new SelectListItem { Text = "North Dakota (ND)", Value = "ND" },
            new SelectListItem { Text = "Ohio (OH)", Value = "OH" },
            new SelectListItem { Text = "Oklahoma (OK)", Value = "OK" },
            new SelectListItem { Text = "Oregon (OR)", Value = "OR" },
            new SelectListItem { Text = "Pennsylvania (PA)", Value = "PA" },
            new SelectListItem { Text = "Rhode Island (RI)", Value = "RI" },
            new SelectListItem { Text = "South Carolina (SC)", Value = "SC" },
            new SelectListItem { Text = "South Dakota (SD)", Value = "SD" },
            new SelectListItem { Text = "Tennessee (TN)", Value = "TN" },
            new SelectListItem { Text = "Texas (TX)", Value = "TX" },
            new SelectListItem { Text = "Utah (UT)", Value = "UT" },
            new SelectListItem { Text = "Vermont (VT)", Value = "VT" },
            new SelectListItem { Text = "Virginia (VA)", Value = "VA" },
            new SelectListItem { Text = "Washington (WA)", Value = "WA" },
            new SelectListItem { Text = "West Virginia (WV)", Value = "WV" },
            new SelectListItem { Text = "Wisconsin (WI)", Value = "WI" },
            new SelectListItem { Text = "Wyoming (WY)", Value = "WY" },
            new SelectListItem { Text = "District of Columbia (DC)", Value = "DC" }
        };
    }
}