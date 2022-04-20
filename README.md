# EmailTest
With this tool, you can try to connect to an email server and validate the credentials.

## Usage
1. Download the latest [Release](https://github.com/TeraNovaLP/EmailTest/releases) and unzip it.
2. Go in the unzipped folder and call the executable like this to get an overview over all options and arguments: `.\EmailTest.exe -h`.

### Options
| Name         | Description                                                                  |
| ------------ | ---------------------------------------------------------------------------- |
| -m           | Mode: Imap, Smtp.
| -s           | Establish a secure connection.
| -stls        | Use STARTTLS to secure the connection.
| -sv          | Skip certificate validation.
| -d           | Debug
| -h           | Help information.

### Example
`.\EmailTest.exe -m Imap address port username password`
