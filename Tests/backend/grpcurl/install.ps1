$installDir='~\.gc'

if(Test-Path -Path $installDir)
{}
else
{
    mkdir $installDir
}

cp conf.ps1 $installDir
