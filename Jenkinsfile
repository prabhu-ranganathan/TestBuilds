pipeline
{
    environment{
        workspacefile = 'workspace'
    }
    agent any
    stages
    {
        stage('Checkout from SCM'){
            steps{
                echo "Clearing out the existing workspaces"
                cleanWs()
                echo 'Check-out from Git SCM'
                checkout scm
                echo "code copied to workspace location : ${workspace}"
            }
        }
        stage('Build'){
            steps{
                echo 'Initializing the build'
                bat "echo Current workspace is ${workspace}"
                bat "powershell.exe -file ./build.ps1 -Configuration Debug -Target Build"
                echo 'End of the build'
            }
        }
        stage('Deploy')
        {
            steps{
                echo 'Deploy'
            }
        }
    }
}
