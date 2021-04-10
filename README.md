# FlightSimulator

Overview and Features: 

Our project consists of a GUI interface (using WPF application and .NET framework) which will allow us to view and control the plane in FlightGear simulator. By recieving the data of a flight, we are able to simulate it through FlightGear and analyze several attributes related to the flight, such as throttle, altitude, airspeed, etc. Moreover, we are able to view the joystick of the plane to help us inspect the direction the plane is heading, along with a few graphs that help us visually evaluate the various aspect of the flight: the first graph is a view of the attribute selected in the attribute box, the second graph is a view of the attribute most correlated to the attribute we selected, the third graph shows the linear regression between the two attributes from the first two graphs, and the fourth graph displays anomalies from the flight in relation to the two attributes. Lastly, we developed a control panel that aids us in controlling the play speed of the flight, pause or play the simulation, and fast forward/backtrack. 
The project runs on multiple threads and was developed using MVVM architechture.

Structure of the Project: 

Our project is organized in two folders: 

1. WpfApp1- the main folder, contains the main window, view model, and model. 
2. controls- contains all of the controls in our main window, such as the joystick, graphs, and control panel.

Required Installations: 

1. Recompile dll on your own native environment 
2. Import oxyplot in appropriate files in order to view graphs
3. Install the latest version of FlightGear on your computer 
4. Execute the solution! 

Manual: 

1. Run the app and a GUI should open.
2. Upload your CSV and XML files and press Upload File on your right hand side. 
3. The simulation will begin, and you are free to use the controls and sliders to analyze the flight 

UMLs and Class Diagrams: 

Our desktop application consists of 3 main parts that communicate and run. The first component is the MyFlightModel that interacts with the server via TCP communication. The second component is the ViewModel that sends data requests to the MyFlightModel and recieves notifications when data changes from the MyFlightModel. Our last component is the View (MainWindow file) that sends commands to the ViewModel and gets notified about changed data from the ViewModel. Data is displayed in our MainWindow through the process of data binding. The following link is our project UML: 

Short Video About Our Project: 

