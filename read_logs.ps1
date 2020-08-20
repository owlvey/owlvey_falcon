Remove-Item -Recurse -Force ./logs

$POD_NAMES = kubectl get pods -l key=owlvey-api-pod -o=custom-columns=name:.metadata.name -n owlvey

$POD_NAMES | ForEach-Object -Process {    
    kubectl cp  -n owlvey  owlvey/${_}:/app/logs ./logs/${_}     
} 

foreach( $pod in $POD_NAMES){
    if (Test-Path ./logs/${pod}) 
    {
        Get-ChildItem -Path ./logs/${pod} -Name -Include *.log | ForEach-Object -Process {
            $file = $_
            foreach($line in Get-Content ./logs/${pod}/$file) {                
                if($line.ToLower() -Match  "exception" -and -Not $line.ToLower() -Match "/health" ){                    
                    Add-Content -Path ./logs/${pod}/summary.txt -Value $line                           
                }
            }
        }        
    }
}


    




