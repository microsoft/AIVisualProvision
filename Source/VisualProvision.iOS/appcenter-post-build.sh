#!/usr/bin/env bash
#
# Compiles a Xamarin.UITest project, and runs it as a test run in AppCenter. iOS version
#
################################################################################################
##### NOTE: TO RUN THE TESTS, YOU NEED TO DEFINE THE "ENABLE_UITESTS" ENVIRONMENT VARIABLE #####
################################################################################################
#
# Environment variables :
#
# - APPCENTER_TOKEN. You need an AppCenter API token. Instructions on how to get it in https://docs.microsoft.com/en-us/appcenter/api-docs/
# - XAMARIN_UITEST_VERSION. Version of the Xamarin.UITest NuGet package the project is using. Defaults to 2.2.7
# - DEVICES. ID or IDs of devices or device sets previously created in AppCenter. Defaults to "iPhone 8, iOS 12.1" (de95e76a)
# - CUSTOM_LOCALE. Locale. Defaults to "en_US"
# - CUSTOM_TEST_SERIES. Name of test series. Defaults to "connect18"
#
# NOTE: UI_TEST_TOOLS_DIR is where "test-cloud.exe" is. By default in AppCenter is /Users/vsts/.nuget/packages/xamarin.uitest/<xamarin uitest version>/tools

if [ -z "$ENABLE_UITESTS" ]; then
	echo "ERROR! You need to define in AppCenter the ENABLE_UITESTS environment variable. UI Tests will not run. Exiting..."
	exit 0
fi

UITEST_PROJECT_PATH="$APPCENTER_SOURCE_DIRECTORY/MagnetsMobileClient/VisualProvision.UITest"
UITEST_CSPROJ_NAME="VisualProvision.UITest.csproj"
APPCENTER_PROJECT_NAME="ImageDeploy/iOS"
IPA_PATH="$APPCENTER_OUTPUT_DIRECTORY/VisualProvision.iOS.ipa"

DEFAULT_DEVICES="de95e76a"
DEFAULT_XAMARIN_UITEST_VERSION="2.2.7"
DEFAULT_UI_TEST_TOOLS_DIR="$APPCENTER_SOURCE_DIRECTORY/Source/packages/Xamarin.UITest.*/tools"
DEFAULT_LOCALE="en_US"
DEFAULT_TEST_SERIES="connect18"

if [ -z "$APPCENTER_TOKEN" ]; then
	echo "ERROR! AppCenter API token is not set. Exiting..."
	exit 1
fi

if [ -z "$XAMARIN_UITEST_VERSION" ]; then
	echo "WARNING! XAMARIN_UITEST_VERSION environment variable not set. Setting it to its default. Check the version of Xamarin.UITest you are using in your project"
	UI_TEST_TOOLS_DIR="$DEFAULT_UI_TEST_TOOLS_DIR"
else
	echo "Xamarin UITest version is set to $XAMARIN_UITEST_VERSION"
	UI_TEST_TOOLS_DIR="$DEFAULT_UI_TEST_TOOLS_DIR"
fi

if [ -z "$CUSTOM_LOCALE" ]; then
	echo "CUSTOM_LOCALE environment variable not set. Setting it to its default $DEFAULT_LOCALE"
	CUSTOM_LOCALE="$DEFAULT_LOCALE"
fi

if [ -z "$CUSTOM_TEST_SERIES" ]; then
	echo "CUSTOM_TEST_SERIES environment variable not set. Setting it to its default $DEFAULT_TEST_SERIES"
	CUSTOM_TEST_SERIES="$DEFAULT_TEST_SERIES"
fi

if [ -z "$DEVICES" ]; then
	echo "WARNING! Devices variable not set. You need to previously create a device set, and specify it here, eg: <project_name>/iPhonesWithNotch"
	echo "Defaulting to iPhone 8, iOS 12.1 (de95e76a)"
	DEVICES=$DEFAULT_DEVICES
fi

echo "UI Test .csproj path: $UITEST_CSPROJ_PATH"
echo "UI Test Tools Dir: $UI_TEST_TOOLS_DIR"

echo "### Restoring UITest NuGet packages"
msbuild $UITEST_PROJECT_PATH/$UITEST_CSPROJ_NAME /t:restore

echo "### Compiling UITest project"
msbuild $UITEST_PROJECT_PATH/$UITEST_CSPROJ_NAME /t:build /p:Configuration=Release

echo "### Launching AppCenter test run"
appcenter test run uitest --app $APPCENTER_PROJECT_NAME --devices $DEVICES --app-path $IPA_PATH --test-series $CUSTOM_TEST_SERIES --locale $CUSTOM_LOCALE --build-dir $UITEST_PROJECT_PATH/bin/Release --uitest-tools-dir $UI_TEST_TOOLS_DIR --token $APPCENTER_TOKEN --async
