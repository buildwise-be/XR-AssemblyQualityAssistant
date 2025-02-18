# Base d'application XR pour contrôle qualité/assistance montage
Ce répertoire met à disposition du secteur AEC la base d'une application permettant de visualiser en réalité mixte un ensemble d'étapes de contrôle qualité ou de montage/assemblage. Les étapes sont constituées d'un titre, d'instructions, d'une illustration, et d'un modèle 3D associé. L'utilisateur peut ainsi parcourir ces étapes dans l'ordre pour suivre une série d'instructions qui le guident pas à pas pour réaliser la tâche souhaitée. 

## Matériel et logiciel
L'application présentée est à destination du casque Hololens2, de Microsoft. Il est cependant possible de l'adapter pour l'utiliser sur d'autres casques (Quest, Magic Leap 2, par exemple) moyennant l'utilisation de packages adaptés (voir la documentation ciblée, ex. le SDK de Meta, AR Foundation pour les tablettes ou smartphones, etc.) pour gérer les interactions gestuelles et vocales et la lecture QR codes, la base de l'application restant identique.

Le moteur de jeu utilisé pour ceci est [Unity](https://unity.com/), version 6000.0.25f1 (compatibilité avec d'autres versions non testée).

## Méthodologie
L'application se base sur l'utilisation de ScriptableObjects pour stocker les instructions d'assemblage ou de contrôle qualité.
Pour incorporer ses propres instructions, il suffit de créer un nouveau fichier de ce type en faisant un click droit dans la fenêtre de projet, et en choisissant Create > Buildwise > AssemblyProject.

<img src="https://github.com/user-attachments/assets/1fb5c91c-b95e-45bb-91e1-691cab5ca5cd" alt="drawing" width="500"/>

On peut ensuite y ajouter les étapes qu'on souhaite via l'éditeur, et entrer les instructions voulues à chaque étape. Une illustration peut également être fournie.

<img src="https://github.com/user-attachments/assets/643931b3-ece7-4afc-8653-85c9921a5cbf" alt="drawing" width="500"/>

## Compiler l'application (Hololens2)
1. Compiler pour la plateforme "Universal Windows"
2. Ouvrir le projet compilé dans Visual Studio 2022
3. Compiler pour Master ARM64
4. Déployer sur Hololens2 avec câble

## Remerciements :pray:
Identification du cas d'usage, des fonctionnalités, et développements initiés dans le cadre du projet XRFab2Built, avec le soutient de la Wallonie, de Digital Wallonia, et de Wallonie Relance.
Projet porté par Buildwise, en collaboration avec CAP Construction et Techlink.
![image](https://github.com/user-attachments/assets/8ad170b4-74b7-4ca3-a475-606b9fb8a365)
![image](https://github.com/user-attachments/assets/e04d2667-b60e-4fd0-80ca-b48b66e36daa)![image](https://github.com/user-attachments/assets/c4e1d783-26c5-485c-b915-f4d41c84c72a)

![image](https://github.com/user-attachments/assets/de0c6f5a-a76d-4b8a-a488-a90f0c9bef3d)![image](https://github.com/user-attachments/assets/dc69d3c5-9735-43dd-97a6-84d702a3a3f7)![image](https://github.com/user-attachments/assets/9cd9b4e3-d01c-4e21-80ed-ee19e10d968c)





Based on the work of [Joost van Schaik](https://github.com/LocalJoost).
