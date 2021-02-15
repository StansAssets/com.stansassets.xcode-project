# XCode Project
The package is a little helper that allows you to add additional settings and parameters to the XCode project during the Unity post-process phase. 

For example, if you have an IOS plugin that requires some flags, frameworks, libraries, or pList values to be added to the XCode project, there is no need to add it manually every time or making a post-process script.  Just open plugin settings and do it visually.

And sure thing, there is a [C# API](https://api.stansassets.com/xcode-project/) as well, which you can use anytime from your Editor code.

[![NPM Package](https://img.shields.io/npm/v/com.stansassets.ios-deploy)](https://www.npmjs.com/package/com.stansassets.xcode-project)
[![openupm](https://img.shields.io/npm/v/com.stansassets.xcode-project?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.stansassets.xcode-project/)
[![Licence](https://img.shields.io/npm/l/com.stansassets.ios-deploy)](https://github.com/StansAssets/com.stansassets.xcode-project/blob/master/LICENSE)
[![Issues](https://img.shields.io/github/issues/StansAssets/com.stansassets.ios-deploy)](https://github.com/StansAssets/com.stansassets.xcode-project/issues)


[API Reference](https://api.stansassets.com/xcode-project/) | [Forum](https://myforum) | [Wiki](https://github.com/StansAssets/com.stansassets.xcode-project/wiki)

### Install from NPM
* Navigate to the `Packages` directory of your project.
* Adjust the [project manifest file](https://docs.unity3d.com/Manual/upm-manifestPrj.html) `manifest.json` in a text editor.
* Ensure `https://registry.npmjs.org/` is part of `scopedRegistries`.
  * Ensure `com.stansassets` is part of `scopes`.
  * Add `com.stansassets.ios-deploy` to the `dependencies`, stating the latest version.

A minimal example ends up looking like this. Please note that the version `X.Y.Z` stated here is to be replaced with [the latest released version](https://www.npmjs.com/package/com.stansassets.xcode-project) which is currently [![NPM Package](https://img.shields.io/npm/v/com.stansassets.xcode-project)](https://www.npmjs.com/package/com.stansassets.xcode-project).
  ```json
  {
    "scopedRegistries": [
      {
        "name": "npmjs",
        "url": "https://registry.npmjs.org/",
        "scopes": [
          "com.stansassets"
        ]
      }
    ],
    "dependencies": {
      "com.stansassets.xcode-project": "X.Y.Z",
      ...
    }
  }
  ```
* Switch back to the Unity software and wait for it to finish importing the added package.

### Install from OpenUPM
* Install openupm-cli `npm install -g openupm-cli` or `yarn global add openupm-cli`
* Enter your unity project folder `cd <YOUR_UNITY_PROJECT_FOLDER>`
* Install package `openupm add com.stansassets.ios-deploy`

