# Windows Service With QuartzNet
Two job descriptions were made for trial purposes. Jobs will run on windows service application in different time periods.

First job : It will be triggered every 5 seconds.

Second job : It will be triggered every 10 seconds.

## How to create windows service step by step

SC Create "ServiceName" binpath="Exe Path"

**Open Terminal - Run As Administrator**

> SC CREATE "CronJob" binpath="C:\Users\${Username}\repos\DenemeService\DenemeService\bin\Debug\DenemeService.exe"

## How to delete windows service step by step

SC DELETE "ServiceName"

Open Terminal - Run As Administrator 

> SC DELETE "ServiceName"


## Cron Expression Example

- 0 * * ? * *	Every minute
- 0 */2 * ? * *	Every even minute
- 0 1/2 * ? * *	Every uneven minute
- 0 */2 * ? * *	Every 2 minutes
- 0 */3 * ? * *	Every 3 minutes
- 0 */4 * ? * *	Every 4 minutes

## Generator
[Cron Expression Generator](https://www.freeformatter.com/cron-expression-generator-quartz.html)
