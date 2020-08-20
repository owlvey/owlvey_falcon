kubectl apply -f ./envoy.yaml 
kubectl apply -f ./deploy-jaeger.yaml 

Start-Sleep -s 7

kubectl get pods -n owlvey 

$POD_NAMES = kubectl get pods -l key=owlvey-api-pod -o=name -n owlvey


$POD_NAMES | ForEach-Object -Process {            
    $temp = $_.replace("pod/", "")
    kubectl delete pod -n owlvey  ${temp}
} 

Start-Sleep -s 5

kubectl get pods -n owlvey 

$target = kubectl get pods -l key=owlvey-envoy-pod -n owlvey -o name
$target = $target.replace("pod/", "")
write-host $target

# kubectl port-forward $target -n owlvey 10000:10000