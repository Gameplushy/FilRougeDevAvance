# FilRougeDevAvance

Projet conçu par Victor Florent

# Installation
Le client obtiendra une application WPF. Ce logiciel sera connecté à une application serveur ainsi qu’une API.

Veuiller démarrer le serveur (ChatServer) et l'API (APICTL) en localhost avant l'application (Interface).

# Utilisation
Connexion (login-mdp) : admin-admin ou test-sosis
Le bouton start/stop permet de démarrer/arrêter la simulation, en fonction de la règle choisie.
Les règles possèdent le schéma suivant : ##A##D, # étant des nombres.
Les nombres avant A représentent le nombres de voisins qu’une cellule vivante doit avoir pour rester vivant. Les nombres avant D représentent la même chose, mais pour une cellule morte, afin de naître.
L’écran à droite permet de dialoguer avec les autres utilisateurs.
Pour proposer une nouvelle règle, envoyez « RULE=# », # étant la nouvelle règle (voir le format ci-dessus)
