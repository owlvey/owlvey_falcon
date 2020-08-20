kubectl delete -f ./envoy.yaml
kubectl delete -f ./deploy-jaeger.yaml
Start-Sleep -s 5
kubectl apply -f ../service-api.yaml

$POD_NAMES = kubectl get pods -l key=owlvey-api-pod -o=name -n owlvey

$POD_NAMES | ForEach-Object -Process {            
    $temp = $_.replace("pod/", "")
    kubectl delete pod -n owlvey  ${temp}
} 