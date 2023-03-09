# Intensity/Concentration Database to CSV Comparison Tool

Notes for operation:

  1) Concentrations Final With Details must be exported as Samples need to have the DF taken into account. All Database values take the DF into account.
     Without Concentrations Final, all sample concentration values will evaluate to false. 
     
  2) All RS2 values are being skipped for now. There is a flaw in our RS2 calculations when they are generated for the Cal Curves which causes the value to not
     match the calculated value in the database. Needs more investigation and can be re-enabled later when the RS2 discrepency is solved. 
