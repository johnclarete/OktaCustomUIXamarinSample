Introduction
This is a sample of an app with Authentication and Authorisation via OKTA. It uses the Authorization Code Flow with PKCE using custom ui and okta apis to achieve this.

Prerequisite
-You have an Developer Okta Org setup.
-You have created an Application in the org for this app.

Getting Started
To get the sample app running update the App.xaml.cs file with your own Okta Org and Client Id of the Application that you have setup in OKTA.
```
oktaService.ClientId = "????????";
oktaService.OktaOrg = "https://dev-??????.okta.com";
```
Ensure that you have whitelisted the following for your Application in Okta
```
oktaService.RedirectUri = "com.myappnamespace.exampleapp:/callback";
```