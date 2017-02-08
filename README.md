# Gosso.EPiServerAddOn.QuickNavExtension

[![Platform](https://img.shields.io/badge/Episerver%20CMS-9+-green.svg?style=flat)](http://world.episerver.com/cms/) [![Platform](https://img.shields.io/badge/Episerver-%2010.0-green.svg?style=flat)](http://world.episerver.com/cms/) (compiled with 9.0, tested with 10.0.1)

An Episerver addon that adds up to menu items to the QuickNavigationMenu when logged in on public site, link to admin, link to ContentType, and logout.

![alt text](https://github.com/LucGosso/Gosso.EPiServerAddOn.QuickNavExtension/blob/master/QuickNavExtension.gif?raw=true "This is how the QuickNavExtension could look")

# Installation and configuration 

There is a nuget package available under the releases tab. This can be installed via the package manager console in Visual Studio.

Put it in your local nuget feed, Run "install-package Gosso.EPiServerAddOn.QuickNavExtension" in package manager console.

The files will be saved into your modulesbin folders. And not referenced, not needed. Remember to checkin the modulesbin in repository.

To activate content type in menu, add this appsettings: 

    <appSettings>
        <add key="Gosso.QuickNav" value="admin,contenttype,logout" />
    </appSettings>

To only show logout item, add this appsettings: 

    <appSettings>
        <add key="Gosso.QuickNav" value="logout" />
    </appSettings>

You need to have WebAdmins role to see Admin mode menu items. 

You may map Administrators role to WebAdmins like this in episerverframework.config:
  
    <episerver.framework>
    <virtualRoles addClaims="true">
    <providers>
          <add name="WebAdmins" type="EPiServer.Security.MappedRole, EPiServer.Framework" roles="Administrators" mode="Any" />
    </providers>
    </virtualRoles>
    </episerver.framework>
