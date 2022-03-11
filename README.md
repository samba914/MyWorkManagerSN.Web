# MyWorkManagerSN.Web


### Les options

Il se peut que certains utilisateurs ne veuillent pas voir activer tous les modules optionelles que propose l'application.
Il est donc possible de les paramétrer dans Paramètres > Options.
Pour ajouter une nouvelle option il faut : 
* ajouter d'abord l'option au niveau de la classe AccountOptions (Pour le moment toutes les propriétés de la classe AccountOptions sont de type bool)
* au niveau de la vue Profile/ShowOptions.cshtml, il faut rajouter dans le dictionnaire optionsName les valeurs : (nom de la propriété de l'option, le texte à afficher)
```
Exemples:
Je rajoute dans AccountOptions la propriété ActiveSubWithAmount de type bool
il faut donc rajouter dans le dictionnaire optionsName qui se trouve dans  Profile/ShowOptions.cshtml, les valeurs : ("ActiveSubWithAmount", "Abonnement avec un montant")
```
