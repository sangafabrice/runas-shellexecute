/** @file admin.js */

RunAsAdministrator(WSH.Arguments(0), WSH.Arguments(1));

/** 
 * @param {string} programExe - the path to the program to elevate
 * @param {string} command - the arguments used to run the program
 */
function RunAsAdministrator(programExe, command) {
  var Shell32 = new ActiveXObject('Shell.Application');
  Shell32.ShellExecute(programExe, command, null, 'runas');
  Shell32 = null;
}