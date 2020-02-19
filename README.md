# OpenALPR Milestone Plugin #

* The OpenALPR Milestone Plug-in integrates OpenALPR data with Milestone XProtect.  

### How it works ### 

* OpenALPR software reads video directly from a camera stream, analyzes the video, produces JSON data containing plate information, and posts this to a local beanstalkd queue
* The Milestone Plug-in contains two components:
  1. A Windows service that runs in the background.  It constantly reads from the queue and posts the data to Milestone XProtect
  2. A Milestone plug-in that is located inside XProtect that is used to display the results to users
* The Windows service sends two types of data to Milestone XProtect
  1. All recognized license plates are saved as Milestone Bookmarks
  2. Any plates that match an alert list (managed as a CSV spreadsheet) are saved as a Milestone Alarm.
  
The OpenALPR software must be licensed with a commercial key, the Milestone XProtect Plugin simply requires a Milestone license to operate.  

### Minimum Requirements ###

OpenALPR software requires sufficient CPU to recognize license plates.  We recommend a desktop/server class Intel PC with at least 1 CPU core per LPR camera stream being actively analyzed.

In order to use the bookmarks feature (to search plates within Milestone) you must have at least Milestone XProtect Professional+

### Install Instructions ###

Basic Setup
--------------

* Download and install the OpenALPR On-Premises Agent
  - Sign up for an account (for licensing purposes) here: https://cloud.openalpr.com
* Setup the OpenALPR software to use the data destination "Other HTTP Server"
  - Configure the agent to point to http://[xprotectserver]:48125/
* Add your LPR camera to OpenALPR
  - Once this is complete, you should see the license plates being read in the user interface as vehicles drive past the camera
* Install the OpenALPR Milestone Plugin using the installer on the system running Milestone XProtect
* Open Milestone XProtect and click on the "OpenALPR" tab
* Click on the Camera Mapping, and ensure that the OpenALPR camera is properly associated with the milestone camera.  If not, you can click the drop-down to manually associate the two cameras
* As new plates arrive you will be able to search them in Milestone.

Alarm Configuration
---------------------

The OpenALPR Milestone plug-in can also create Milestone alarms based on a list of plates that you provide.  This is managed as a simple spreadsheet.

* On the "OpenALPR" tab of Milestone XProtect, click "Edit Alarms"
* The file will open as a spreadsheet using Excel or a text editor
* Add all the plate numbers that you wish to alert.  Click Save (overwriting the file), and make sure you keep the "CSV" format.
* The OpenALPR plug-in will automatically detect the changes to the CSV file and begin alerting on the plates in the list.


### Developer Instructions ###

* Download Visual Studio 2017
* Install .NET Framework 4.6.2 https://www.microsoft.com/net/download/visual-studio-sdks
* Install NSIS (for packaging the installer)
* Open the project, and switch to "Release" and "x64" mode (Build -> Configuration Manager) 
* If you wish for every build to automatically copy the binaries to your local Milestone system:
  * Copy the "Solution build referenced files\BuildRequirements\*.bat to your build folder"

### Installer Packaging ###

This plug-in uses the NSIS installer to create a Windows installer package.  The nsi file is included in this repo in the installer directory.

### Contribution guidelines ###

You must accept the Contributor License Agreement (CLA) so that we can accept your submissions to the project.

### Who do I talk to? ###

Please direct any questions to support@openalpr.com
