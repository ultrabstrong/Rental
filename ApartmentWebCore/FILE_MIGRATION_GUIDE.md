# File Migration Guide for ApartmentWebCore wwwroot

## Overview
This guide shows you exactly which files to copy from your ApartmentWeb project to the new ApartmentWebCore wwwroot folder structure.

## Directory Structure Created
```
ApartmentWebCore/
??? wwwroot/
?   ??? css/
?   ?   ??? bootstrap.min.css                 ? Copy from ApartmentWeb\Content\
?   ?   ??? bootstrap-icons.css              ? Copy from ApartmentWeb\Content\
?   ?   ??? jquery-ui.min.css               ? Copy from ApartmentWeb\Content\themes\base\
?   ?   ??? bootstrap-select.min.css        ? Copy from ApartmentWeb\Content\
?   ?   ??? custom.css                      ? Already copied ?
?   ??? js/
?   ?   ??? jquery.min.js                   ? Copy from ApartmentWeb\Scripts\
?   ?   ??? bootstrap.bundle.min.js         ? Copy from ApartmentWeb\Scripts\
?   ?   ??? jquery-ui.min.js               ? Copy from ApartmentWeb\Scripts\
?   ?   ??? jquery.validate.min.js         ? Copy from ApartmentWeb\Scripts\
?   ?   ??? jquery.validate.unobtrusive.min.js ? Copy from ApartmentWeb\Scripts\
?   ?   ??? bootstrap-select.min.js        ? Copy from ApartmentWeb\Scripts\
?   ?   ??? custom/
?   ?       ??? apply-form.js              ? Already copied ?
?   ?       ??? apply-loading.js           ? Copy from ApartmentWeb\Scripts\custom\
?   ?       ??? maintenance-form.js        ? Copy from ApartmentWeb\Scripts\custom\
?   ?       ??? modalmanager.js            ? Copy from ApartmentWeb\Scripts\custom\
?   ?       ??? requireif.js               ? Copy from ApartmentWeb\Scripts\custom\
?   ?       ??? visibletoggle.js           ? Copy from ApartmentWeb\Scripts\custom\
?   ?       ??? validationErrorStyle.js    ? Copy from ApartmentWeb\Scripts\custom\
?   ?       ??? phone-masking.js           ? Copy from ApartmentWeb\Scripts\custom\
?   ?       ??? ssn-masking.js             ? Copy from ApartmentWeb\Scripts\custom\
?   ?       ??? datepicker-config.js       ? Copy from ApartmentWeb\Scripts\custom\
?   ??? images/
?       ??? webicon.ico                    ? Copy from ApartmentWeb\Images\
```

## Copy Commands (if using command line)

### CSS Files
```bash
# From ApartmentWeb directory
copy "Content\bootstrap.min.css" "..\ApartmentWebCore\wwwroot\css\"
copy "Content\bootstrap-icons.css" "..\ApartmentWebCore\wwwroot\css\"
copy "Content\themes\base\jquery-ui.min.css" "..\ApartmentWebCore\wwwroot\css\"
copy "Content\bootstrap-select.min.css" "..\ApartmentWebCore\wwwroot\css\"
```

### JavaScript Core Files
```bash
# Find the exact jQuery file name first (it might be jquery-3.x.x.min.js)
copy "Scripts\jquery*.min.js" "..\ApartmentWebCore\wwwroot\js\jquery.min.js"
copy "Scripts\bootstrap.bundle.min.js" "..\ApartmentWebCore\wwwroot\js\"
copy "Scripts\jquery-ui*.min.js" "..\ApartmentWebCore\wwwroot\js\jquery-ui.min.js"
copy "Scripts\jquery.validate.min.js" "..\ApartmentWebCore\wwwroot\js\"
copy "Scripts\jquery.validate.unobtrusive.min.js" "..\ApartmentWebCore\wwwroot\js\"
copy "Scripts\bootstrap-select.min.js" "..\ApartmentWebCore\wwwroot\js\"
```

### Custom JavaScript Files
```bash
copy "Scripts\custom\apply-loading.js" "..\ApartmentWebCore\wwwroot\js\custom\"
copy "Scripts\custom\maintenance-form.js" "..\ApartmentWebCore\wwwroot\js\custom\"
copy "Scripts\custom\modalmanager.js" "..\ApartmentWebCore\wwwroot\js\custom\"
copy "Scripts\custom\requireif.js" "..\ApartmentWebCore\wwwroot\js\custom\"
copy "Scripts\custom\visibletoggle.js" "..\ApartmentWebCore\wwwroot\js\custom\"
copy "Scripts\custom\validationErrorStyle.js" "..\ApartmentWebCore\wwwroot\js\custom\"
copy "Scripts\custom\phone-masking.js" "..\ApartmentWebCore\wwwroot\js\custom\"
copy "Scripts\custom\ssn-masking.js" "..\ApartmentWebCore\wwwroot\js\custom\"
copy "Scripts\custom\datepicker-config.js" "..\ApartmentWebCore\wwwroot\js\custom\"
```

### Images
```bash
copy "Images\webicon.ico" "..\ApartmentWebCore\wwwroot\images\"
# Copy any other images you have in the Images folder
```

## Missing Files to Download
If any files are missing from your ApartmentWeb project, you can download them:

### Bootstrap Icons CSS
If bootstrap-icons.css is missing, download from:
https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.2/font/bootstrap-icons.css

### Bootstrap 5 Files (if needed)
- CSS: https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css
- JS: https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js

### jQuery Files (if needed)
- jQuery: https://code.jquery.com/jquery-3.7.1.min.js
- jQuery UI: https://code.jquery.com/ui/1.13.2/jquery-ui.min.js
- jQuery UI CSS: https://code.jquery.com/ui/1.13.2/themes/ui-lightness/jquery-ui.css
- jQuery Validation: https://cdn.jsdelivr.net/npm/jquery-validation@1.19.5/dist/jquery.validate.min.js
- jQuery Unobtrusive: https://cdn.jsdelivr.net/npm/jquery-validation-unobtrusive@4.0.0/dist/jquery.validate.unobtrusive.min.js

## Verification
After copying, your wwwroot structure should look like this and the layout should work without errors.

## Next Steps
1. Copy all the files listed above
2. Test your application
3. Update individual pages to use page-specific scripts via @section scripts
4. Remove the placeholder images/placeholder.txt file after copying real images

## Page-Specific Scripts
For pages that need specific scripts, use this pattern:

```razor
@section scripts {
    <script src="~/js/custom/apply-form.js"></script>
}
```

# jQuery UI Images Fix

The jQuery UI CSS file references image files that need to be copied to maintain the theme icons.

## Required Images Directory Structure
```
ApartmentWebCore/
??? wwwroot/
    ??? css/
        ??? images/
            ??? ui-icons_444444_256x240.png
            ??? ui-icons_555555_256x240.png  
            ??? ui-icons_777777_256x240.png
            ??? ui-icons_cc0000_256x240.png
            ??? ui-icons_ffffff_256x240.png
            ??? ui-icons_777620_256x240.png
```

## Copy Commands
If you have the original jQuery UI theme images in ApartmentWeb:
```bash
# Create the images directory first
mkdir "ApartmentWebCore\wwwroot\css\images"

# Copy all jQuery UI images (if they exist in original project)
copy "ApartmentWeb\Content\themes\base\images\*.png" "ApartmentWebCore\wwwroot\css\images\"
```

## Alternative: Download Missing Images
If images don't exist in your original project, download them from jQuery UI CDN:
- Download from: https://code.jquery.com/ui/1.12.0/themes/base/images/
- Save each PNG file to `ApartmentWebCore\wwwroot\css\images\`

## Images You Need:
1. `ui-icons_444444_256x240.png` - Default icons  
2. `ui-icons_555555_256x240.png` - Hover state icons
3. `ui-icons_777777_256x240.png` - Button icons
4. `ui-icons_ffffff_256x240.png` - Active state icons
5. `ui-icons_cc0000_256x240.png` - Error state icons
6. `ui-icons_777620_256x240.png` - Highlight state icons