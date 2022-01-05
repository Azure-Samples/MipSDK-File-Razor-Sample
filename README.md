# MIP SDK Razor Sample

This ASP.NET Core Razor application demonstrates using MIP SDK to label files on download, restrict upload based on label, and set policies around upload/download.

## Features

This project framework provides the following features:

* Demonstrates labeling a file on download.
* Demonstrates performing `isProtected()` and `AccessCheck` then generating a decrypted file for upload. 

## Getting Started

### Prerequisites

- Windows 10 or 11
- MIP SDK 1.11 or later
- Visual Studio 2022

### App Registration

To allow clients to authenticate against the web application, as well as to enable the web application to connect on behalf of clients, a new application registration must be configured in the **Azure AD management portal**.

### Create an Azure AD App Registration

> Skip this step if you've already created a registration for previous sample. You may continue to use that client ID.

1. Go to https://portal.azure.com and log in as a global admin.
> Your tenant may permit standard users to register applications. If you aren't a global admin, you can attempt these steps, but may need to work with a tenant administrator to have an application registered or be granted access to register applications.
2. Click Azure Active Directory, then **App Registrations** in the menu blade.
3. Click **New Registration**
4. Give the application a name.
5. Under *Supported account types* select **Accounts in this directory only**
6. Click **Register**

The application registration will be created. 

### Add API Permissions

1. Select **API permissions**.
2. Select **Add a permission**.
3. Select **Microsoft APIs** if it's not already selected.
4. Select **Azure Rights Management Services**.
5. Select **Applications Permissions** and then **Content.DelegatedReader** and **Content.DelegatedWriter**.
6. Select **Add permissions**.
7. Again, select **Add a permission**.
8. Select **APIs my organization uses**.
9. In the search box, type **Microsoft Information Protection Sync Service** then select the service.
10. Select **Application permissions**.
11. Check **UnifiedPolicy.Tenant.Read**.
12. Select **Add permissions**.
13. In the **API permissions** blade, Select **Grant admin consent for <Your Tenant>** and confirm.

### Set Redirect URI

1. Select **Authentication**.
2. Select **Add a platform**.
3. Select **Web**
4. For the **Redirect URI** enter: `https://localhost:7143/signin-oidc`
5. For the **Front Channel Logout ID** enter: `https://localhost:44347/oidc-signout`
6. Check the **ID Tokens** box. 
7. Click **Save**

### Create an App Secret

1. Select **Certificates and Secrets**.
2. Click **New Client Secret**
3. Provide a description and set an expiration period.
4. Click **Add**.
5. Copy the value somewhere safe.
> For production applications, it's recommended that you use certificates instead of app secrets.

### Quickstart

1. git clone https://github.com/Azure-Samples/Mip-FileSdk-Razor-Sample.git
2. cd Mip-FileSdk-Razor-Sample
3. .\Mip-FileSdk-Razor-Sample.sln
4. Restore NuGet packages by right-clicking on the solution and selected **Restore NuGet Packages**