# Touching Function

This is the repository for Touching Function.
Part of UWB CRSC research of Summer 2022.

## Current Limitation and Future Work
- Multiple graph axes
	- Current implementation only contains 1 graphing canvas in the world, consider allowing users to create graphing canvas on demand

- Including support for implicit functions
	- Current implementation cannot interpret implicit functions such as ‘x + y = 4’

- Adding z axis for 3D functions
	- Current implementation only graphs 2D functions, consider adding in the z-axis to allow graphing of 3D functions

- Further refinement of UI layout
	- Ex. refinement for point creation/deletion UI (currently, it is difficult to move the cursor to the exact location of the point to be deleted)

- Landmarks and/or minimap to ease user navigation
	- Objects can be added to the Unity scene as landmarks for the users to refer to when they are unsure of their current position in the scene
	- A minimap can be added to the UI that shows the user’s position relative to the rest of the scene
	- A third person perspective camera can be added when the user is near a graphed function to make it easier for the user to track whether they are standing on the graphed function

- Manipulation of function’s slope at a selected point
	- A feature where users can select a point and manipulate the slope at the point resulting in changing the graphed function altogether

- Displaying function properties for a selected interval
	- A feature where users can select an interval along a graphed function and get more information regarding the function’s properties (such as definite integral) only within the selected interval
	- User can also add annotation and notes for the selected intervals

- Graphing the derivative of a graphed function
	- The derivative function is connected to the current function and any changes made to one is reflected in the other function




## Creating and Updating a WolframAlpha AppId

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
 
 
## Getting the WolframAlpha nuget package for new projects

1. Download NuGet from https://github.com/GlitchEnzo/NuGetForUnity
2. Drag the download object into your unity scene and click "install". If correctly done a new dropdown tab should appear between "Component" and "Window".
3. Click NuGet->Manage NuGet Packages.                                                                                 
 ![image](https://github.com/UWB-ARSandbox/2022_6_TouchFunction/blob/main/README%20Pictures/Pic6.png)
 
4. Click the "Show Prerelease" box and enter "wolframalpha" into the search bar. 
5. Install "Genbox.WolframAlpha" by Ian Qvist 
 ![image](https://github.com/UWB-ARSandbox/2022_6_TouchFunction/blob/main/README%20Pictures/Pic7.PNG)
 
A full API for Genbox.WolframAlpha can be found at https://github.com/Genbox/WolframAlpha
