# docker run --name api-integration-test --rm localhost:48700/registry/api-integration-test:latest
# docker run --name api-integration-test --rm --entrypoint dotnet localhost:48700/registry/api-integration-test:latest test

kubectl delete -f ./cluster/job-integration-test.yaml 

kubectl apply -f ./cluster/job-integration-test.yaml 

kubectl wait --for=condition=complete job/owlvey-api-integration-test-job -n owlvey --timeout=60s

$POD_NAME = kubectl get pods -l key=owlvey-api-integration-test-job -o=name -n owlvey

kubectl logs ${POD_NAME} -n owlvey
kubectl logs ${POD_NAME} -n owlvey > ./logs/integration_test.log

# kubectl cp -n owlvey ${POD_NAME}:/app ./logs/integration-test/ 






