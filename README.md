# TP2 - Jeux

## I- Analyse et justification des cas de test

### A- Identification des cas de test

3 tests basiques sont faits :
1. un jeu démarre et un coup est joué
2. un jeu reprend une partie déjà commencée
3. on vérifie un gagnant

Puis suivent quelques tests d'erreur :
1. jeu non terminé, impossible de déterminer le vainqueur
2. état du jeu invalide
3. nom du joueur invalide
4. coup invalide

### B- Priorisation des scénarios

Les premiers tests sont critiques et vérifient le déroulé d'une partie de jeu, puis les suivants contrôlent la gestion d'erreurs.  

## II- Architecture et représentation des données

### A- Lisibilité des données de test

Des tables ont été utilisées pour représenter l'état des jeux.  
Elles sont plus pratiques que des chaînes de caractères pour stocker un historique de tours.  
Et le format avec des colonnes est adapté pour séparer plusieurs joueurs.  

Les données de la table sont passées au jeu, qui sait comment les lire.  

### B- Extensibilité

L'interface `IGame` regroupe plusieurs fonctions basiques pour contrôler le déroulement d'un jeu tour par tour.  
On peut ajouter un nouveau jeu similaire sans modifier les step definitions, et on écrira sees tests avec le même vocabulaire que les autres.  
Pour ajouter un nouveau jeu, il suffit d'ajouter une entrée dan la factory `GameFactory`, qui construit le jeu selon le nom de la `Feature` Gherkin.  

## III- Stratégie BDD et bonnes pratiques

### A- Langage ubiquitaire

Le choix a été fait d'éviter le vocabulaire spécifique pour simplifier l'extensibilité et éviter la duplication de code dans les step definitions.  

### B- Réutilisabilité

Aucune step definition spécifique, pour les raisons énoncées au-dessus.  

### C- Maintenance

Pour ajouter un jeu, il suffit de créer une nouvelle classe implémentant `IGame`, dee créer une entrée dans la factory `GameFactory`, puis d"ajouter un fichier de tests sur le même modèle que pour les autres jeux, et c'est tout.  
Donc facile à maintenir et à faire évoluer.  
