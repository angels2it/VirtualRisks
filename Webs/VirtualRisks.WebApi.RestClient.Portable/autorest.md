input-file: 
  - http://localhost:61764/swagger/docs/v1
csharp: # just having a 'csharp' node enables the use of the csharp generator.
  namespace: VirtualRisks.WebApi.RestClient #override the namespace 
  output-folder : ./ # relative to the global value.