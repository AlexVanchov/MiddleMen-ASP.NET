# MiddleMan


[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

MiddleMan is web service for trading accounts, selling game items and buying items
# Features:
  - Accounts with full access (e-mail change and password)
  - Game items (CS:GO, Dota 2 , etc)



### Tech

* [ASP.NET] - MVC
* [node.js] - evented I/O for the backend

### TODO:

 * Admin edit reordering
    * Make hidden form with name="categoryId" and name="orderNumber" with Submint btn which is on the page where the JavaSctipt will fill with the orderNumber
 * Home page listingitems in category
 * onclick item displaying
 
 ## Objects relations:
* BaseOnDeleteModel addon
    * IsDeleted(bool)
    * DeletedOn(nullable DateTime)
* BaseOnCreateModel addon
    * CreatedOn(DateTime)
    * ModifiedOn(nullable DateTime)
* Categoty : BaseOnDeleteModel, BaseOnCreateModel
    * Id(hash-string)
    * Name(string)
* Offer : BaseOnDeleteModel, BaseOnCreateModel
    * Id(hash-string)
    * CategoryId(foreign key(Category))
    * Name(string)
    * Price(decimal)
    * Description(string)
    * picUrl(nullable string)

License
----

MIT


**Open Source**

   [node.js]: <http://nodejs.org>
   [ASP.NET]: <https://dotnet.microsoft.com/apps/aspnet>
