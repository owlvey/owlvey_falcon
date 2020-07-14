
$POD_NAMES = kubectl get pods -l key=owlvey-api-pod -o=custom-columns=name:.metadata.name -n owlvey

$POD_NAMES | ForEach-Object -Process {
    
    kubectl cp  -n owlvey  owlvey/${_}:/app/logs ./logs/${_}     
} 