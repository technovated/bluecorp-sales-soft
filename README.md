# bluecorp-sales-soft

A Restful API based event-driven solution that manages sale order dispatch workflow seemlessly. 

## Table of Contents

- [Architecture] (#architecture)
- [Future-Target] (#targets)
- [Deployment] (#deployment)
- [Effortspent] (#Effortspent)

## Architecture
It is a event-driven, cloud-ready, cloud-agnostic containerized cross-platform solution. It is designed and developed with best practices adhering clean code, CQRS pattern with loosly coupled, interface-segregated, injectable modules and components on demand, runtime injections. Security and safety by design and development.


## Targets
This solution is developed cloud-agnostic containers, unfortunately full-fledged cloud integration is not possible now. Targets=> 1. Requires Azure subscription, 2. configure Static IP and whitelist on Azure SFTP, 3. Integration with Azure AppInsights for comprehensive observability, 4. Implement AAD for security are some of the future plans targeted.

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
