# MyWorkManagerSN.Web

### Opérations base de donnée
La majeure partie des requêtes vers la bases de donnée se sont à travers la classe générique DBManager<T> qui se trouve dans la bibliothèque Service.
Pour ajouter un element dans la base de donnée : 
* on utilise la méthode Add qui prend en parèmetre l'élément à ajouter. Ex d'utilsation 
```
  Category cat = new Category(){Code= "test", Label="test", UserId= "f545fe-fezf"};
  new DbManager<Category>().Add(cat);
```

 Pour modifier un élément il y a plusieurs méthodes: 
  * la méthode Update(E element) qui fonctionne de la même manière que le Add. Avantage: seul les champs modifiés sont considérés donc gain de performance!
  * la méthode GlobalUpdate(E element) cette méthode est appelée de la même manière que la métthode Update simple. ça particularité est le fait que pour la modification toutes les propriétés de l'élement seront parcourues et non pas seulement les champs modifiés. En général pour une modification dont l'un des champs modifiés est un champ de type objet (owned), on utilisera cette méthode pour faire la modification.
  * la méthode UpdateValue(string elementId,string userId,string property, object value), cette méthode permet la modification d'un seul champs d'un objet. (champs de type primitif). 
  ```
  elementId : l'id de l'élément à modifier
  userId : l'id du user connecté
  property : le nom de la proprité à modifier. Ex: Label
  value : la nouvelle valeur à mettre
  Ex: DbManager<Category>().UpdateValue("da4fz8-fzfe", "f545fe-fezf", "Label", "New Val");
  ```
  
  Pour tous autres méthodes, voir le code
  

### Composants Js
#### autocomplete
Ce composant permet d'avoir une liste de choix correspondant à un texte lorqu'on tape un texte dans un input.
Mode d'utilisation
```
  Templae : 
 <div class="autocomplete" style="display: inline-flex; width: -webkit-fill-available;">
    <input id="inputAutoComplete" class="form-control input-default" data-controller="Category" type="text" required name="myCountry" placeholder="Catégorie" autocomplete="off">   </div>
  Script : 
  autocomplete(document.getElementById("inputAutoComplete"));
```
* il faut créer une div avec la classe autocomplete
* puis dans cette div y  rajouter un input avec un id donné ex : inputAutoComplete. 
* dans le input il faut rajouter comme attribut, l'attribut data-controller. Il correspond au controller qui sera appelé pour chercher les informations
* on peut aussi ajouter l'attribut data-method qui correspond à la méthode qui sera appelée. Par défaut c'est la méthode "GetAll" du controller qui est appelée.
```
Exemple : 
  Avec
  <input id="inputAutoComplete" class="form-control input-default" data-controller="Category" type="text" autocomplete="off"> 
  C'est l' url suivante qui sera appelée : "/Category/GetAll" qui renvoie donc toutes les catégories présentes dans la base et créées par le user currrent
  
  Avec 
  <input id="inputAutoComplete" class="form-control input-default" data-controller="Order" data-methode="GetOrderByState?state='pending'" type="text" autocomplete="off"> 
  C'est l' url suivante qui sera appelée : "/Order/GetOrderByState?state='pending" qui renvoie donc toutes les commandes ayant le status en cours et créées par le user currrent
```
  

  
### Les options

Il se peut que certains utilisateurs ne veuillent pas voir activer tous les modules optionels que propose l'application.
Il est donc possible de les paramétrer dans Paramètres > Options.
Pour ajouter une nouvelle option il faut : 
* ajouter d'abord l'option au niveau de la classe AccountOptions (Pour le moment toutes les propriétés de la classe AccountOptions sont de type bool)
* au niveau de la vue Profile/ShowOptions.cshtml, il faut rajouter dans le dictionnaire optionsName les valeurs : (nom de la propriété de l'option, le texte à afficher)
```
Exemples:
Je rajoute dans AccountOptions la propriété ActiveSubWithAmount de type bool
il faut donc rajouter dans le dictionnaire optionsName qui se trouve dans  Profile/ShowOptions.cshtml, les valeurs : ("ActiveSubWithAmount", "Abonnement avec un montant")
```
