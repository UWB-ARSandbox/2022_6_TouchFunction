This file will walk you through creating a WolframAlpha AppId for use in the file //TOENTER

1. go to https://developer.wolframalpha.com/portal/signup.html and create an account. A free non-commercial account will be fine.
2. verify the email on your account. Wolfram will not recognize your AppId until this is done.
3. Go to https://developer.wolframalpha.com/portal/myapps/ and click "Get an AppId" near the top righthand corner.
4. Enter the application name and description and click "Get AppId"
5. Copy the "APPID" (second entry) in the pop-up window before clicking "OK"
	If you clicked "OK" before copying the AppId, click the "Edit" button below your app name and copy the AppID. 
6. In file //TOENTER, replace the AppId in all WolframAlphaClient declarations in quotation marks and save the file. 

If you are creating a new project and wish to utilize this functionality, you will need to do the following:

1. Download NuGet from https://github.com/GlitchEnzo/NuGetForUnity
2. Drag the download object into your unity scene and click "install". If correctly done a new dropdown tab should appear between "Component" and "Window".
3. Click NuGet->Manage NuGet Packages.
4. Click the "Show Prerelease" box and enter "wolframalpha" into the search bar. 
5. Install "Genbox.WolframAlpha" by Ian Qvist 

A full API for Genbox.WolframAlpha can be found at https://github.com/Genbox/WolframAlpha
