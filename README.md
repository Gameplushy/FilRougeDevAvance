# FilRougeDevAvance

Projet conçu par Victor Florent

# Installation
Le client obtiendra une application WPF. Ce logiciel sera connecté à une application serveur ainsi qu’une API.

Voir le zip dans l'onglet Releases du Git

Veuiller démarrer le serveur (ChatServer) et l'API (APICTL) en localhost avant l'application (Interface).

Pour l'API, vous devez au préalable changer la variable d'environnement ASPNETCORE_ENVIRONMENT avant de démarrer l'exécutable.
Utilisez la commande `set ASPNETCORE_ENVIRONMENT` suivi de `Development`, `Qualif` ou `Prod` avant de démarrer l'API par console. 

# Utilisation
Connexion (login-mdp) : admin-admin ou test-sosis
Le bouton start/stop permet de démarrer/arrêter la simulation, en fonction de la règle choisie.
Les règles possèdent le schéma suivant : ##A##D, # étant des nombres.
Les nombres avant A représentent le nombres de voisins qu’une cellule vivante doit avoir pour rester vivant. Les nombres avant D représentent la même chose, mais pour une cellule morte, afin de naître.
L’écran à droite permet de dialoguer avec les autres utilisateurs.
Pour proposer une nouvelle règle, envoyez « RULE=# », # étant la nouvelle règle (voir le format ci-dessus)
Les autres utilisateurs pourront répondre soit par "y" (oui) ou "n" (non). Répondre non annulera entièrement la nouvelle règle. Lorsque tous les utilisateurs (sauf celui ayant proposé la règle) auront répondu oui, la nouvelle règle sera transmise à tout le monde.