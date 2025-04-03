# bluecorp-sales-soft

A Restful API based event-driven solution that manages sale order dispatch workflow seemlessly. 

## Table of Contents

- [Architecture] (#Architecture)
- [Future-Target] (#Targets)
- [Deployment] (#deployment)
- [Effortspent] (#Effortspent)
- [Reference guide] (##Reference guide)

## Architecture
It is a event-driven, cloud-ready, cloud-agnostic containerized cross-platform solution. It is designed and developed with best practices adhering clean code, CQRS pattern with loosly coupled, interface-segregated, injectable modules and components on demand, runtime injections. Security and safety by design and development.

Key architecural platforms: Restful APIs, Azure Blob storage, RabbitMQ, .NET core Middleware, Logger 


## Targets
This solution is developed cloud-agnostic containers, unfortunately full-fledged cloud integration is not possible now. Targets=> 1. Requires Azure subscription, 2. configure Static IP and whitelist on Azure SFTP, 3. Integration with Azure AppInsights and log analytics for comprehensive observability and monitoring, 4. Implement AAD for security, 5.Azure API Management are some of the future plans targeted.

## deployment
This solution expects .env file containing secrets for execution and functioning of the application.
API_KEY=##
SFTP_HOST=##
SFTP_USER=##
SFTP_PASSWORD=##
SFTP_FOLDER=##
AzureStorage=##

## Effortspent

 Solution design and Infrastructure - 15 hrs
 Developement and testing - 8 hrs

 ## Reference guide
 Documents/Developer%20test%20handbook.docx
