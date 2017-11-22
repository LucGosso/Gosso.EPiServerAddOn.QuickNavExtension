# Gosso.EPiServerAddOn.QuickNavExtension
**version 2.0 (2017-11-22)**

[![Platform](https://img.shields.io/badge/Platform-.NET%204.5.2-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx) [![Platform](https://img.shields.io/badge/Episerver%20CMS-9+-green.svg?style=flat)](http://world.episerver.com/cms/) [![Platform](https://img.shields.io/badge/Episerver-%2010.0-green.svg?style=flat)](http://world.episerver.com/cms/) (compiled with 9.0, tested with 10.4, Use Version 1.3) 

[![Platform](https://img.shields.io/badge/Platform-.NET%204.6.1-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx) [![Platform](https://img.shields.io/badge/Episerver-%2011.1-green.svg?style=flat)](http://world.episerver.com/cms/) (version 2.0)

An Episerver addon that adds menu items to the QuickNavigationMenu when logged in on public site, 
All configurable links to imagevault, find, admin, admin content type, and logout. Even custom links!

![alt text](https://github.com/LucGosso/Gosso.EPiServerAddOn.QuickNavExtension/blob/master/QuickNavExtension.png?raw=true "This is how the QuickNavExtension could look")

# Installation and configuration 

Available on nuget.episerver.com http://nuget.episerver.com/en/OtherPages/Package/?packageId=Gosso.EPiServerAddOn.QuickNavExtension

Also there is a nuget package available under the releases tab. This can be installed via the package manager console in Visual Studio.

Put it in your local nuget feed, Run "install-package Gosso.EPiServerAddOn.QuickNavExtension" in package manager console.

The files will be saved into your modulesbin folders. And not referenced, not needed. Remember to checkin the modulesbin in repository.

Default menus are Admin and logout, to activate other menu items apply this appsettings: (they are sortable)

    <appSettings>
        <add key="Gosso.QuickNav" value="imagevault,find,admin,contenttype,logout" />
    </appSettings>

You can add custom menu items, Name and url with pipe in between. Name can be resource path eg /shell/admin/logout

    <appSettings>
        <add key="Gosso.QuickNav" value="Custom link|http://devblog.gosso.se,imagevault,find,admin,contenttype,logout" />
    </appSettings>

To only show logout item, apply this appsettings: 

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
