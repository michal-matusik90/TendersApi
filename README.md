# Task description
Your task will be to create a REST API application that meets the following requirements:

REST API should provide users access to data available in https://tenders.guru/pl/api with some additional functionalities. Please find below technical and functional requirements.

## Technical Requirements:

* Each endpoint must support pagination.
* The application must be able to run locally.
* The amount of data handled by the application should be limited to the first 100 pages of the source API.

## Functional Requirements:

* User should be able to retrieve tenders.
* User should be able to filter tenders by price in EUR.
* User should be able to retrieve tenders ordered by price in EUR.
* User should be able to filter tenders by date.
* User should be able to retrieve tenders ordered by date.
* User should be able to retrieve tenders won by a given supplier by providing supplier’s ID
* User should be able to retrieve tender with a given ID.

## Quality Requirements:

* Scalability
* Maintainability
* Testability
* Readability

## Response should include:

* Tender’s ID
* Tender’s Date
* Tender’s Title
* Tender’s Description
* Tender’s Amount in EUR
* Tender’s suppliers
* Supplier’s ID
* Supplier’s Name


# Running code locally

## Prerequisites:

* Azurite Storage Emulator
https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio%2Cblob-storage#install-azurite