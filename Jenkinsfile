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
                #!checkout([$class: 'GitSCM', branches: [[name: '*/master']], doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], userRemoteConfigs: [[url: 'https://github.com/prabhu-ranganathan/TestBuilds']]])
                checkout scm
                echo "code copied to workspace location : ${workspace}"
            }
        }
        stage('Build'){
            steps{
                echo 'Initializing the build'
                bat "echo Current workspace is ${workspace}"
                bat "D:\\nuget\\nuget.exe restore TestBuild.sln"
                bat "\"${tool 'msbuild core'}\" TestBuild\\TestBuild.csproj"
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