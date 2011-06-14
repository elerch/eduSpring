EduSpring: Learn about IoC and DI concepts, with discussion about implementation in Spring.NET
==============================================================================================

Follow the projects in order.  You'll need to set each one as the startup project in turn.

1. **Advantages of DI**:  This introduces you to why you'd want a DI container.  This is a bit hard to describe in code, so read through the 
                          comments in the code to get a sense of the problem.  Once understanding it a bit better, change the command line in 
						  the project properties to switch the dependency, which is how a real system might do it.

2. **IoCWithoutSpring**:  This introduces you to what an IoC container really is: a dictionary with string keys and object values.
                          The comments will guide you through how each section is dealth with by Spring.Net (and probably other IoC containers)

3. **FallaciesOfInterfaces**: DI requires using Interfaces rather than concrete classes.  But this doesn't absolve you from the 
                              issues inherent with swapping out implementations.  Details in how each implementation behave
						      can trip you up.  Here are some of the basic ways in which that can happen.

