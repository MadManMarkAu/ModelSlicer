# ModelSlicer
Slices a model file horizontally, drawing lines where the model polygons intersect with with the slicing plane.

This project demonstrates how to intersect a plane with triangular geometry. This project will load the "teapot.obj" geometry file, and allow you to slice through the object geometry along an axis-aligned X/Z plane, with variable Y position.

The intersecting triangles are converted to line segments, to show the intersecting segment.

Scrolling the trackbar left and right will adjust the intersecting plane's Y position, from the geometry's minimum X coordinate, to the geometry's maximum Y coordinate.
