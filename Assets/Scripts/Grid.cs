using UnityEngine;
using System.Collections;

public class Grid : ScriptableObject {
	public enum GridType {
		NormalGrid,
		UnaccessableGrid,
		StartGrid,
		EndGrid
	};

	public enum GridState{
		NotProcessed,
		InCloseList,
		InOpenList
	};
	public GridType gridType;	
	public GridState gridState;
	public Grid fromGrid;
	public static int GRID_SIZE = 1;
	public Vector2 pointOfGrid;
	public int movementCost;
	public bool isOccupied;
	/*@property (nonatomic)CGRect rect;
	@property (nonatomic)CGPoint pointOfGrid;
	@property (nonatomic, retain)NSColor *color;
	@property (nonatomic)GridType gridType;
	@property (nonatomic)int movementCost;
	@property (nonatomic)int movementCostWithHeuristic;
	@property (nonatomic)GridState gridState;
	@property (nonatomic, retain)Grid* fromGrid;*/
	public Grid(){

	}
	public void setPointOfGrid(int x, int y){
		pointOfGrid.x = x;
		pointOfGrid.y = y;
	}
	public Grid(Grid copy){
		Grid clone = new Grid ();
		clone.gridType = copy.gridType;
		clone.fromGrid = copy.fromGrid;
		clone.gridState = copy.gridState;
		clone.pointOfGrid = copy.pointOfGrid;
		clone.movementCost = copy.movementCost;
	}
}
