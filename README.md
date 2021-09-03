# AgentCommunicationSolution

This solution provides the following:
  AgentCommunicationService - Rest API that serves eDoc and eMessage transmission files. This service also provides the capability of transmitting the transmission files.
  AgentCommunicationUtility - Client application that processes arguments for execution requests and interacts with its respective service.
  DDS.DCP - Common library for shared resources.
  AgentCommunicationTests - Unit and integration tests of the client and server

What this solution does not contain:
  Emessage Simulation - Emessage integration would consume the api from it's IIB message flow to get transmission content and then transmit. From a CSVParser perspective, for each record found in a billing data source it would do the same. I would suggest creating an integration unit test to ensure emessages are getting generated and persisted to their destination.
  Internal Integrations - The service relies on a few integrations in order to provide content to the consumer. I did not add integrations with the Agent Activation database, calls to the CLAS and PLUS services or calls to the IVANS API.
  File transmission - I did add functionality to Persist files to disk but not the consumer implementation.
  
This design should seperate responsibility between client and server, allow for bettwe testing of the interfaces, components and view models and improve performance of the process and entire workflow in general. It is in no way complete but the idea is there.

As I mentioned before, please hit me up anytime if you hae any questions.

Andres
