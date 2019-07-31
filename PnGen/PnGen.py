import sys
import os
import glob
import subprocess
import shutil

def png_gen(txt_file, pallet_file):
    print('Processing {}'.format(txt_file))
    exe_path = os.path.join(r'\\siradel.local', 'Atlas', 'Radio', 'Resources', 'Softwares', 'Development', 'PnGen', 'PnGen.exe')
    png_path = txt_file.replace('.txt', '.png')
    if not os.path.exists(png_path.replace('.png', '.iso.png')):
        if os.path.exists(png_path):
            shutil.copy(png_path, png_path.replace('.png', '.iso.png'))

    subprocess.run([exe_path, '-d', txt_file, '-p', pallet_file, '-qn', 'received_power', '-qu', 'db_mw', '-o', png_path])

    results_dir = os.path.abspath(os.path.dirname(txt_file))
    references_dir = os.path.abspath(os.path.join(results_dir, '..', 'references'))
    png_filename = os.path.basename(png_path)
    compare_png(references_dir, results_dir, png_filename)

def compare_png(references_dir, results_dir, filename):
    exe_path = os.path.join(r'\\siradel.local', 'Atlas', 'PROJECTS', '201206 InteractiveViewer3D', 'Product', 'Libraries', 'ImageComparator', 'master', '9-4265e76e2e', 'Release', 'CompareImageFiles.exe')
    print(filename)
    # compare with png from res folder
    # output norm diff in res folder
    subprocess.run([exe_path, os.path.join(references_dir, filename), os.path.join(results_dir, filename), os.path.join(results_dir, filename + '_diff.png'), '100', '0'])

if __name__ == '__main__':
    scenarii_dir = os.path.dirname(os.path.abspath(__file__))

    # Iterate scenarii results
    for root, dirs, files in os.walk(scenarii_dir):
        for dirname in dirs:
            if dirname == 'Results' or dirname == 'results':
                # get txt list from results folder
                for f in glob.glob(os.path.join(root, dirname, '*.txt')):
                    png_gen(f, r'D:\svn\working directory\volcano_3_enterprise\Tests\resources\pallets\volcano_def.RES.vpf')
    os.system('pause')