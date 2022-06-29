# 2022_6_TouchFunction

This is the repository for Touching Function.
Part of UWB CRSC research of Summer 2022.


Creating and Updating a WolframAlpha AppId for use in the file WolframAlpha.cs

1. go to https://developer.wolframalpha.com/portal/signup.html and create an account. A free non-commercial account will be fine.
2. verify the email on your account. Wolfram will not recognize your AppId until this is done.
3. Go to https://developer.wolframalpha.com/portal/myapps/ and click "Get an AppId" near the top righthand corner.

 ![image](https://github.com/UWB-ARSandbox/2022_6_TouchFunction/blob/main/README%20Pictures/Pic1.PNG)
 
4. Enter the application name and description and click "Get AppId"
5. Copy the "APPID" (second entry) in the pop-up window before clicking "OK"
 ![image](https://github.com/UWB-ARSandbox/2022_6_TouchFunction/blob/main/README%20Pictures/Pic3.PNG)
 
	If you clicked "OK" before copying the AppId, click the "Edit" button below your app name and copy the AppID. 
 ![image](https://github.com/UWB-ARSandbox/2022_6_TouchFunction/blob/main/README%20Pictures/Pic4.PNG)
 
6. In file WolframAlpha.cs, replace the AppId in all WolframAlphaClient declarations in quotation marks and save the file. 
 ![image](https://github.com/UWB-ARSandbox/2022_6_TouchFunction/blob/main/README%20Pictures/Pic5.PNG)
 
 
If you are creating a new project and wish to utilize this functionality, you will need to do the following:

1. Download NuGet from https://github.com/GlitchEnzo/NuGetForUnity
2. Drag the download object into your unity scene and click "install". If correctly done a new dropdown tab should appear between "Component" and "Window".
3. Click NuGet->Manage NuGet Packages.

 ![image](https://github.com/UWB-ARSandbox/2022_6_TouchFunction/blob/main/README%20Pictures/Pic6.png)
 
4. Click the "Show Prerelease" box and enter "wolframalpha" into the search bar. 
5. Install "Genbox.WolframAlpha" by Ian Qvist 
 ![image](https://github.com/UWB-ARSandbox/2022_6_TouchFunction/blob/main/README%20Pictures/Pic7.PNG)
 
A full API for Genbox.WolframAlpha can be found at https://github.com/Genbox/WolframAlpha
